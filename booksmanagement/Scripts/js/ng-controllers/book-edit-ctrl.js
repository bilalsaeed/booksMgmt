myApp.controller('BookEditCtrl', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm) {

    $scope.form = {};

    var uppy = Uppy.Core({
        debug:
            true,
        autoProceed:
            false,
        restrictions:
        {
            maxNumberOfFiles:
                1,
            allowedFileTypes:
                ['.pdf']
        }
    });

    var uppy_soft = Uppy.Core({
        debug:
            true,
        autoProceed:
            false,
        restrictions:
        {
            allowedFileTypes:
                ['.pdf']
        }
    });

    uppy.use(Uppy.Dashboard, {
        inline: true,
        target: '#uppyUploader_partcode',
        replaceTargetContent: true,
        showProgressDetails: true,
        note: '1 document only',
        height: 150,
        width: 300,
        hideUploadButton: true,
        locale: {
            strings: { dropPaste: 'Upload part code paper, %{browse}' },
        },
        browserBackButtonClose: true,
        proudlyDisplayPoweredByUppy: false
    });
    //uppy.use(Uppy.Tus, { endpoint: root + '/tus' });

    uppy_soft.use(Uppy.Dashboard, {
        inline: true,
        target: '#uppyUploader_softcopy',
        replaceTargetContent: true,
        showProgressDetails: true,
        note: '1 document only',
        height: 150,
        width: 300,
        hideUploadButton: true,
        locale: {
            strings: { dropPaste: 'Upload book soft copy, %{browse}' },
        },
        browserBackButtonClose: true,
        proudlyDisplayPoweredByUppy: false
    });
    //uppy_soft.use(Uppy.Tus, { endpoint: root + '/tus' });

    $scope.getCars = function () {
        $http.get(root + 'api/Cars/GetCars').then(function success(response) {
            $scope.cars = response.data;
            $scope.carOriginal = response.data;
            console.log('cars:', $scope.cars);
        }, function error() { });
    }

    $scope.getCarParts = function () {
        $http.get(root + 'api/CarParts/GetCarParts').then(function success(response) {
            $scope.carParts = response.data;
            $scope.carPartsOriginal = response.data;
            console.log('carParts:', $scope.carParts);
        }, function error() { });
    }

    $scope.getCarPartComponents = function () {
        $http.get(root + 'api/CarPartComponents/GetCarPartComponents').then(function success(response) {
            $scope.carPartComponents = response.data;
            $scope.carPartComponentsOriginal = response.data;
            console.log('carPartComponents:', $scope.carPartComponents);
        }, function error() { });
    }

    $scope.getCarPartComponentDescs = function () {
        $http.get(root + 'api/CarPartComponentDescs/GetCarPartComponentDescs').then(function success(response) {
            $scope.carPartComponentDescs = response.data;
            $scope.carPartComponentDescsOriginal = response.data;
            console.log('carPartComponentDescs:', $scope.carPartComponentDescs);
        }, function error() { });
    }

    $scope.bookTypes = [
        { Name: "Soft copy", Id: 1 },
        { Name: "Hard copy", Id: 2 }
    ];

    var pathname = window.location.pathname.split("/");
    $scope.bookId = pathname[pathname.length - 1];

    $scope.getBook = function () {
        $http.get(root + 'api/Books/GetBook/' + $scope.bookId).then(function success(response) {
            $scope.form = response.data;
            console.log('book data:', $scope.form);
        }, function error() { });
    }

    $scope.getCars();
    $scope.getCarParts();
    $scope.getCarPartComponents();
    $scope.getCarPartComponentDescs();
    $scope.getBook();



    //Other logical functions here

    $scope.carPartComponentsFilter = function (item) {
        if ($scope.form.CarPartId && $scope.form.CarId)
            return item.CarPartId === $scope.form.CarPartId && item.CarPart.CarId === $scope.form.CarId

        if ($scope.form.CarId)
            return item.CarPart.CarId === $scope.form.CarId

        return item;
    };

    $scope.carPartComponentDescsFilter = function (item) {
        if ($scope.form.CarPartComponentId && $scope.form.CarPartId && $scope.form.CarId)
            return item.CarPartComponentId === $scope.form.CarPartComponentId
                && item.CarPartComponent.CarPartId === $scope.form.CarPartId
                && item.CarPartComponent.CarPart.CarId === $scope.form.CarId

        if ($scope.form.CarPartId && $scope.form.CarId)
            return item.CarPartComponent.CarPartId === $scope.form.CarPartId
                && item.CarPartComponent.CarPart.CarId === $scope.form.CarId

        if ($scope.form.CarId)
            return item.CarPartComponent.CarPart.CarId === $scope.form.CarId

        return item;
    };

    $scope.carSelected = function () {

    }

    $scope.partSelected = function () {
        var part = $filter("filter")($scope.carPartsOriginal, { Id: $scope.form.CarPartId });
        $scope.form.CarId = part[0].CarId;
    }

    $scope.componentSelected = function () {
        var component = $filter("filter")($scope.carPartComponentsOriginal, { Id: $scope.form.CarPartComponentId });
        $scope.form.CarPartId = component[0].CarPartId;
        $scope.form.CarId = component[0].CarPart.CarId;
    }

    $scope.componentDescSelected = function () {
        var componentDesc = $filter("filter")($scope.carPartComponentDescsOriginal, { Id: $scope.form.CarPartComponentDescId });
        $scope.form.CarPartComponentId = componentDesc[0].CarPartComponentId;
        $scope.form.CarPartId = componentDesc[0].CarPartComponent.CarPartId;
        $scope.form.CarId = componentDesc[0].CarPartComponent.CarPart.CarId;

    }

    $scope.save = function () {
        if (!$scope.form.CarId || !$scope.form.Title || !$scope.form.TypeId) {
            toaster.pop({
                type: 'error',
                title: '',
                body: "Please fill the fields before save.",
            });
            return false;
        }

        //console.log($scope.form);
        $http.post(root + 'api/Books/PutBook', $scope.form).then(function success(response) {
            if (response.status == 204) {

                var link = window.location.href.split('/');
                var bookId = Number(link[link.length - 1])

                uppy.use(Uppy.XHRUpload, { endpoint: root + 'HttpHandlers/FileRequestHandler.ashx?Type=UploadBookPartCode&&BookId=' + bookId })
                uppy.upload().then((result) => {
                    console.log('first', result);
                    uppy_soft.use(Uppy.XHRUpload, { endpoint: root + 'HttpHandlers/FileRequestHandler.ashx?Type=UploadBookSoftCopy&&BookId=' + bookId })
                    uppy_soft.upload().then((res1) => {
                        console.log('second', res1);
                        window.location.href = root + 'Books';
                    });
                });

            }

        }, function error(error) {
                toaster.pop({
                    type: 'error',
                    title: '',
                    body: error.data.Message,
                });
        });
    }

});