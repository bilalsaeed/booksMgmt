
myApp.controller('DrawingOrderNewCtrl', function ($scope, $filter, $http, $location, $uibModal, toaster, $ngConfirm) {


    $scope.getCars = function () {
        $http.get(root + 'api/Cars/GetCars').then(function success(response) {
            $scope.cars = response.data;

            const urlParams = new URLSearchParams(window.location.search);
            if (urlParams.get('car')) {
                var selectedCar;
                var keepGoing = true;
                angular.forEach($scope.cars, function (item, key) {
                    if (keepGoing) {
                        if (item.Id == urlParams.get('car')) {
                            selectedCar = item;
                            keepGoing = false;
                        }
                    }
                });

                if (selectedCar) {
                    $scope.openDrawingOrderModal(selectedCar);
                }
            }

            //console.log('all parts:', $scope.carParts);

        }, function error() { });
    }

    $scope.getCarParts = function () {
        $http.get(root + 'api/CarParts/GetCarParts').then(function success(response) {
            $scope.carParts = response.data;

            const urlParams = new URLSearchParams(window.location.search);
            if (urlParams.get('part')) {
                var selectedPart;
                var keepGoing = true;
                angular.forEach($scope.carParts, function (item, key) {
                    if (keepGoing) {
                        if (item.Id == urlParams.get('part')) {
                            selectedPart = item;
                            keepGoing = false;
                        }
                    }
                });

                if (selectedPart) {
                    $scope.openDrawingOrderModal(selectedPart);
                }
            }

            console.log('all parts:', $scope.carParts);

        }, function error() { });
    }

    //$scope.getCarParts();
    $scope.getCars();


    //Other logical functions here

    $scope.openDrawingOrderModal = function (car) {
        var modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: root + 'Scripts/js/ng-templates/drawingorder-new-template.html',
            controller: 'DrawingOrderCreateCtrl',
            size: 'lg',
            resolve: {
                car: function () {
                    return car;
                }
            }
        });
        //modalInstance.result.then(function () {
        //    //on ok button press 
        //}, function (data) {
        //    $scope.UserPhoto = data;
        //});
    }
});

myApp.controller('DrawingOrderCreateCtrl', function ($scope, $filter, $http, $uibModalInstance, toaster, $ngConfirm, car) {
    $scope.car = car;

    $scope.getCarParts = function () {
        $http.get(root + 'api/CarParts/GetCarParts').then(function success(response) {
            $scope.carParts = response.data;
            $scope.carPartsOriginal = response.data;

            const urlParams = new URLSearchParams(window.location.search);
            if (urlParams.get('part')) {
                var selectedPart;
                var keepGoing = true;
                angular.forEach($scope.carParts, function (item, key) {
                    if (keepGoing) {
                        if (item.Id == urlParams.get('part')) {
                            selectedPart = item;
                            keepGoing = false;
                        }
                    }
                });

                if (selectedPart) {
                    $scope.CarPartId = selectedPart.Id;
                    $scope.getCarPartComponents();
                }
            }

            //console.log('carParts:', $scope.carParts);
        }, function error() { });
    }


    $scope.getCarPartComponents = function () {
        $http.get(root + 'api/CarPartComponents/GetCarPartComponentsByPart?partId=' + $scope.CarPartId).then(function success(response) {
            $scope.carPartComponents = response.data;

            const urlParams = new URLSearchParams(window.location.search);
            if (urlParams.get('comp')) {
                var selectedComp;
                var keepGoing = true;
                angular.forEach($scope.carPartComponents, function (item, key) {
                    if (keepGoing) {
                        if (item.Id == urlParams.get('comp')) {
                            selectedComp = item;
                            keepGoing = false;
                        }
                    }
                });

                if (selectedComp) {
                    $scope.CarPartComponentId = selectedComp.Id;
                }
            }
        }, function error() { });
    }

    $scope.getCarParts();


    //Other logical functions here


    $scope.saveDrawingOrder = function (orderData) {
        $http.post(root + 'api/DrawingOrders/SaveDrawingOrder', orderData).then(function success(response) {
            if (response.status == 200) {
                toaster.pop({
                    type: 'success',
                    title: '',
                    body: "Order is submitted. You can check the status of order from your dashboard.",
                });
                $uibModalInstance.dismiss();
            }
        }, function error(err) {
            toaster.pop({
                type: 'error',
                title: 'Error',
                body: err.data,
            });
        });

    }

    $scope.ok = function () {
        if ($scope.Description && $scope.Location && $scope.Purpose) {
            $ngConfirm({
                title: 'Submit Drawing Order?',
                content: 'Are you sure to submit this Order?',
                autoClose: 'cancel|10000',
                buttons: {
                    submitRequest: {
                        text: 'Submit',
                        btnClass: 'btn-success',
                        action: function () {
                            var orderData = {
                                CarId: $scope.car.Id,
                                CarPartId: $scope.CarPartId,
                                CarPartComponentId: $scope.CarPartComponentId,
                                Description: $scope.Description,
                                Purpose: $scope.Purpose,
                                Location: $scope.Location,
                                JobNumber: $scope.JobNumber
                            }
                            $scope.saveDrawingOrder(orderData);
                        }
                    },
                    cancel: function () {

                    }
                }
            });

        }
        else {
            toaster.pop({
                type: 'error',
                title: '',
                body: "Please fill the fields before submitting order.",
            });
        }
    }
    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    }
});