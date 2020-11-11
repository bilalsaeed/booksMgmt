myApp.controller('BookCreateCtrl', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm) {

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
            maxNumberOfFiles:
                1,
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
        width:300,
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
        { Name:"Soft copy", Id : "1" },
        { Name:"Hard copy", Id : "2"}
    ];

    $scope.getCars();
    $scope.getCarParts();
    $scope.getCarPartComponents();
    $scope.getCarPartComponentDescs();



    //Other logical functions here

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

        //if (uppy.getFiles().length <= 0) {
        //    toaster.pop({
        //        type: 'error',
        //        title: '',
        //        body: "Please upload part code paper.",
        //    });
        //    return false;
        //}


        $http.post(root + 'api/Books/PostBook', $scope.form).then(function success(response) {
            if (response.status == 201) {
                uppy.use(Uppy.XHRUpload, { endpoint: root + 'HttpHandlers/FileRequestHandler.ashx?Type=UploadBookPartCode&&BookId=' + response.data.Id })
                uppy.upload().then((result) => {
                    console.log('first', result);
                    uppy_soft.use(Uppy.XHRUpload, { endpoint: root + 'HttpHandlers/FileRequestHandler.ashx?Type=UploadBookSoftCopy&&BookId=' + response.data.Id })
                    uppy_soft.upload().then((res1) => {
                        console.log('second', res1);
                        window.location.href = root + 'Books';
                    });
                });

            }
            else {
                console.log(response);
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