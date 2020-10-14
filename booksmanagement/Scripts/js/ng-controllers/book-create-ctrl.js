myApp.controller('BookCreateCtrl', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm) {

    $scope.getCars = function () {
        $http.get(root + 'api/Cars/GetCars').then(function success(response) {
            $scope.cars = response.data;
            console.log('cars:', $scope.cars);
        }, function error() { });
    }

    $scope.getCarParts = function () {
        $http.get(root + 'api/CarParts/GetCarParts').then(function success(response) {
            $scope.carParts = response.data;
            console.log('carParts:', $scope.carParts);
        }, function error() { });
    }

    $scope.getCarPartComponents = function () {
        $http.get(root + 'api/CarPartComponents/GetCarPartComponents').then(function success(response) {
            $scope.carPartComponents = response.data;
            console.log('carPartComponents:', $scope.carPartComponents);
        }, function error() { });
    }

    $scope.getCarPartComponentDescs = function () {
        $http.get(root + 'api/CarPartComponentDescs/GetCarPartComponentDescs').then(function success(response) {
            $scope.carPartComponentDescs = response.data;
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

});