﻿myApp.controller('BookCreateCtrl', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm) {

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
                ['.pdf', '.docx', '.doc']
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
                ['.pdf', '.docx', '.doc']
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
    uppy.use(Uppy.Tus, { endpoint: root + '/tus' });

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
    uppy_soft.use(Uppy.Tus, { endpoint: root + '/tus' });


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

        //console.log($scope.form);
        $http.post(root + 'api/Books/PostBook', $scope.form).then(function success(response) {
            if (response.status == 201) {

                uppy.upload().then((result) => {
                    $scope.Files = [];
                    var files = Array.from(result.successful);
                    files.forEach((file) => {
                        var resp = file.response.uploadURL;
                        var id = resp.substring(resp.lastIndexOf("/") + 1, resp.length);
                        var fileObj = {};
                        fileObj.FileId = id;
                        fileObj.Name = file.name;
                        fileObj.Type = 'P';
                        fileObj.Size = file.size;
                        fileObj.ContentType = file.type;
                        fileObj.BookId = response.data.Id;
                        $scope.Files.push(fileObj);
                    });

                    if ($scope.Files.length > 0) {
                        $http.post(root + 'api/Books/PostBookMediaFiles', $scope.Files).then(function success(res) {
                            console.log(res);
                            if (res.status == 200) {
                                uppy_soft.upload().then((result) => {
                                    $scope.Files = [];
                                    var files = Array.from(result.successful);
                                    files.forEach((file1) => {
                                        var r = file1.response.uploadURL;
                                        var id1 = r.substring(r.lastIndexOf("/") + 1, r.length);
                                        var fileObj1 = {};
                                        fileObj1.FileId = id1;
                                        fileObj1.Name = file1.name;
                                        fileObj1.Type = 'S';
                                        fileObj1.Size = file1.size;
                                        fileObj1.ContentType = file1.type;
                                        fileObj1.BookId = response.data.Id;
                                        $scope.Files.push(fileObj1);
                                    });

                                    if ($scope.Files.length > 0) {
                                        $http.post(root + 'api/Books/PostBookMediaFiles', $scope.Files).then(function success(res) {
                                            console.log(res);
                                            if (res.status == 200) {
                                                window.location.href = root + 'Books';
                                            }
                                        }, function error() { });
                                    }
                                    else {
                                        window.location.href = root + 'Books';
                                    }
                                });
                            }
                        }, function error() { });
                    }
                    else {
                        window.location.href = root + 'Books';
                    }
                });

                //window.location.href = root + 'Books';
            }
            
        }, function error() { });
    }

});