myApp.controller('DrawingOrderNewCtrl', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm) {

    $scope.getCarParts = function () {
        $http.get(root + 'api/CarParts/GetCarParts').then(function success(response) {
            $scope.carParts = response.data;
            console.log('all parts:', $scope.carParts);

        }, function error() { });
    }

    $scope.getCarParts();



    //Other logical functions here

    $scope.openDrawingOrderModal = function (part) {
        var modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: root + 'Scripts/js/ng-templates/drawingorder-new-template.html',
            controller: 'DrawingOrderCreateCtrl',
            size: 'lg',
            resolve: {
                part: function () {
                    return part;
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

myApp.controller('DrawingOrderCreateCtrl', function ($scope, $filter, $http, $uibModalInstance, toaster, $ngConfirm, part) {
    $scope.part = part;


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
                                CarPartId: $scope.part.Id,
                                Description: $scope.Description,
                                Purpose: $scope.Purpose,
                                Location: $scope.Location
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