myApp.controller('DrawingOrderListCtrl', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm) {

    $scope.currUser = $('#currUserId').html();

    $scope.getAllOrders = function () {
        $http.get(root + 'api/DrawingOrders/GetAllDrawingOrders').then(function success(response) {
            $scope.orders = response.data;
            //console.log('orders:', $scope.orders);

        }, function error() { });
    }

    $scope.getAllOrders();

    //Other logical functions
    $scope.openViewOrderModal = function (order) {
        var modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: root + 'Scripts/js/ng-templates/view-drawingorder-template.html',
            controller: 'ViewOrderCtrl',
            size: 'lg',
            resolve: {
                order: function () {
                    return order;
                }
            }
        });
        //modalInstance.result.then(function () {
        //    //on ok button press 
        //}, function (data) {
        //    $scope.UserPhoto = data;
        //});
    }

    $scope.assignOrder = function (order) {
        var modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: root + 'Scripts/js/ng-templates/assign-drawingorder-template.html',
            controller: 'AssignOrderCtrl',
            size: 'lg',
            resolve: {
                order: function () {
                    return order;
                }
            }
        });
        modalInstance.result.then(function () {
            //on ok button press 
        }, function (data) {
            $scope.getAllOrders();
        });
    }
    
    $scope.submitDrawing = function (order) {
        var modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: root + 'Scripts/js/ng-templates/submit-drawingorder-template.html',
            controller: 'SubmitOrderCtrl',
            size: 'lg',
            resolve: {
                order: function () {
                    return order;
                }
            }
        });
        modalInstance.result.then(function () {
            //on ok button press 
        }, function (data) {
            $scope.getAllOrders();
        });
    }

    $scope.approveDrawing = function (order) {
        $ngConfirm({
            title: 'Approve Drawing?',
            content: 'Are you sure to approve this drawing?',
            autoClose: 'cancel|10000',
            buttons: {
                submitRequest: {
                    text: 'Submit',
                    btnClass: 'btn-success',
                    action: function () {
                        var orderData = {
                            Id: order.Id
                        }
                        $http.post(root + 'api/DrawingOrders/ApproveDrawingOrder', orderData).then(function success(response) {
                            if (response.status == 200) {
                                toaster.pop({
                                    type: 'success',
                                    title: '',
                                    body: "Drawing is approved.",
                                });
                                $scope.getAllOrders();
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
                },
                cancel: function () {

                }
            }
        });
    }

    $scope.rejectDrawing = function (order) {
        $ngConfirm({
            title: 'Reject Drawing?',
            content: 'Are you sure to reject this drawing?',
            autoClose: 'cancel|10000',
            buttons: {
                submitRequest: {
                    text: 'Submit',
                    btnClass: 'btn-success',
                    action: function () {
                        var orderData = {
                            Id: order.Id
                        }
                        $http.post(root + 'api/DrawingOrders/RejectDrawingOrder', orderData).then(function success(response) {
                            if (response.status == 200) {
                                toaster.pop({
                                    type: 'success',
                                    title: '',
                                    body: "Drawing is rejected. It will be submitted again by the drawer.",
                                });
                                $scope.getAllOrders();
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
                },
                cancel: function () {

                }
            }
        });
    }
});

myApp.controller('ViewOrderCtrl', function ($scope, $uibModalInstance, toaster, $ngConfirm, order) {
    $scope.order = order;

    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    }
});

myApp.controller('AssignOrderCtrl', function ($scope, $uibModalInstance, $http, toaster, $ngConfirm, order) {
    $scope.order = order;

    $scope.getAllUsers = function () {
        $http.get(root + 'api/Books/GetAllUsers').then(function success(response) {
            $scope.users = response.data;
            console.log('orders:', $scope.users);

        }, function error() { });
    }

    $scope.getAllUsers();

    //Other logical functions here

    $scope.assignDrawingOrder = function (orderData) {
        $http.post(root + 'api/DrawingOrders/AssignDrawingOrder', orderData).then(function success(response) {
            if (response.status == 200) {
                toaster.pop({
                    type: 'success',
                    title: '',
                    body: "Order is assigned.",
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


    $scope.save = function () {
        if ($scope.assignedto) {
            $ngConfirm({
                title: 'Assign Drawing Order?',
                content: 'Are you sure to assign this Order?',
                autoClose: 'cancel|10000',
                buttons: {
                    submitRequest: {
                        text: 'Submit',
                        btnClass: 'btn-success',
                        action: function () {
                            var orderData = {
                                Id: $scope.order.Id,
                                AssignedToId: $scope.assignedto
                            }
                            $scope.assignDrawingOrder(orderData);
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

myApp.controller('SubmitOrderCtrl', function ($scope, $uibModalInstance, $http, toaster, $ngConfirm, order) {
    $scope.order = order;

    $scope.submitDrawingOrder = function (orderData) {
        $http.post(root + 'api/DrawingOrders/SubmitDrawingOrder', orderData).then(function success(response) {
            if (response.status == 200) {
                toaster.pop({
                    type: 'success',
                    title: '',
                    body: "Drawing is Submitted.",
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

    $scope.save = function () {
        if (true) {
            $ngConfirm({
                title: 'Submit Drawing?',
                content: 'Are you sure to submit this drawing?',
                autoClose: 'cancel|10000',
                buttons: {
                    submitRequest: {
                        text: 'Submit',
                        btnClass: 'btn-success',
                        action: function () {
                            var orderData = {
                                Id: $scope.order.Id
                            }
                            $scope.submitDrawingOrder(orderData);
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