myApp.controller('DrawingsListCtrl', function ($scope, $filter, $http, $uibModalInstance, toaster, $ngConfirm, Lightbox, carId, carPartId, carPartCompId) {
    
    $scope.carId = carId;
    $scope.carPartId = carPartId;
    $scope.carPartCompId = carPartCompId;

    $scope.getCarBookMediaList = function (id) {
        $http.get(root + 'api/DrawingOrders/GetCarDrawingFiles/' + id).then(function success(response) {
            $scope.files = response.data;
            console.log('drawing files:', $scope.files);
        }, function error() { });
    }

    $scope.getCarPartBookMediaList = function (id) {
        $http.get(root + 'api/DrawingOrders/GetCarPartDrawingFiles/' + id).then(function success(response) {
            $scope.files = response.data;
            //console.log('book files:', $scope.files);
        }, function error() { });
    }

    $scope.getCarPartCompBookMediaList = function (id) {
        $http.get(root + 'api/DrawingOrders/GetCarPartCompDrawingFiles/' + id).then(function success(response) {
            $scope.files = response.data;
            //console.log('book files:', $scope.files);
        }, function error() { });
    }

    $scope.openLightboxModal = function (index) {
        Lightbox.openModal($scope.files, index);
    };

    if ($scope.carPartCompId > 0)
        $scope.getCarPartCompBookMediaList($scope.carPartCompId);
    else if ($scope.carPartId > 0)
        $scope.getCarPartBookMediaList($scope.carPartId);
    else if ($scope.carId > 0)
        $scope.getCarBookMediaList($scope.carId);

    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    }
});