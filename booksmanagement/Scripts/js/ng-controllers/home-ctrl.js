myApp.controller('HomeCtrl', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm) {

    $scope.currUser = $('#currUserId').html();

    $scope.structure = {
        folders: [
            {
                name: 'Folder 1', files: [{ name: 'File 1.jpg' }, { name: 'File 2.png' }], folders: [
                    { name: 'Subfolder 1', files: [{ name: 'Subfile 1' }] },
                    { name: 'Subfolder 2' },
                    { name: 'Subfolder 3' }
                ]
            },
            { name: 'Folder 2' }
        ]
    };

    $scope.options = {
        onNodeSelect: function (node, breadcrums) {
            console.log(node);
        }
    };

    $scope.getCarTree = function () {
        $http.get(root + 'api/DrawingOrders/GetCarTree').then(function success(response) {
            $scope.carTree = response.data;
            console.log('tree:', $scope.carTree);
        }, function error() { });
    }

    $scope.getPendingRequests = function () {
        $http.get(root + 'api/BookBorrow/GetPendingRequests').then(function success(response) {
            $scope.pendingRequests = response.data;
            //console.log('requests:', $scope.pendingRequests);
        }, function error() { });
    }

    $scope.getGrantedRequests = function () {
        $http.get(root + 'api/BookBorrow/GetGrantedRequests').then(function success(response) {
            $scope.grantedRequests = response.data;
            //console.log('requests:', $scope.pendingRequests);
        }, function error() { });
    }

    $scope.getPendingDrawingOrders = function () {
        $http.get(root + 'api/DrawingOrders/GetPendingDrawingOrders').then(function success(response) {
            $scope.pendingDrawingOrders = response.data;
            //console.log('orders:', $scope.pendingDrawingOrders);
        }, function error() { });
    }

    $scope.approveBookRequest = function (requestId) {
        $ngConfirm({
            title: 'Approve Request?',
            content: 'Are you sure to approve this Request?',
            autoClose: 'cancel|10000',
            buttons: {
                submitRequest: {
                    text: 'Approve',
                    btnClass: 'btn-success',
                    action: function () {
                        $http.post(root + 'api/BookBorrow/ApproveBookRequest', requestId).then(function success(response) {
                            if (response.status == 200) {
                                toaster.pop({
                                    type: 'success',
                                    title: '',
                                    body: "Request is Approved. Please grant user the book from requests page.",
                                });
                                $scope.getPendingRequests();
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


    $scope.collectBook = function (requestId) {
        $ngConfirm({
            title: 'Collect this Book?',
            content: 'Are you sure to collect this book back?',
            autoClose: 'cancel|10000',
            buttons: {
                submitRequest: {
                    text: 'Yes',
                    btnClass: 'btn-success',
                    action: function () {
                        $http.post(root + 'api/BookBorrow/CollectBook', requestId).then(function success(response) {
                            if (response.status == 200) {
                                toaster.pop({
                                    type: 'success',
                                    title: '',
                                    body: "Book is collected back.",
                                });
                                $scope.getAllBookRequests();
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


    $scope.getCarTree();
    $scope.getPendingRequests();
    $scope.getGrantedRequests();
    $scope.getPendingDrawingOrders();


    //Other logical functions
    $scope.openViewRequestModal = function (request) {
        var modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: root + 'Scripts/js/ng-templates/view-bookrequest-template.html',
            controller: 'ViewRequestCtrl',
            size: 'lg',
            resolve: {
                request: function () {
                    return request;
                }
            }
        });
        //modalInstance.result.then(function () {
        //    //on ok button press 
        //}, function (data) {
        //    $scope.UserPhoto = data;
        //});
    }

    $scope.openApproveRequestModal = function (request) {
        var modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: root + 'Scripts/js/ng-templates/approve-bookrequest-template.html',
            controller: 'ApproveRequestCtrl',
            size: 'lg',
            resolve: {
                request: function () {
                    return request;
                }
            }
        });
        modalInstance.result.then(function () {
            //on ok button press 
        }, function (data) {
            $scope.getPendingRequests();
        });
    }



    //Drawing order requests here

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
                $scope.getPendingDrawingOrders();
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
                $scope.getPendingDrawingOrders();
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
                                $scope.getPendingDrawingOrders();
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
                                $scope.getPendingDrawingOrders();
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

myApp.controller('ViewRequestCtrl', function ($scope, $uibModalInstance, toaster, $ngConfirm, request) {
    $scope.request = request;

    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    }
});

myApp.controller('ApproveRequestCtrl', function ($scope, $http, $uibModalInstance, toaster, $ngConfirm, request) {
    $scope.request = request;


    $scope.approveBookRequest = function (requestId) {
        
        $ngConfirm({
            title: 'Approve Request?',
            content: 'Are you sure to approve this Request?',
            autoClose: 'cancel|10000',
            buttons: {
                submitRequest: {
                    text: 'Approve',
                    btnClass: 'btn-success',
                    action: function () {
                        $http.post(root + 'api/BookBorrow/ApproveBookRequest', requestId).then(function success(response) {
                            if (response.status == 200) {
                                toaster.pop({
                                    type: 'success',
                                    title: '',
                                    body: "Request is Approved. Please grant user the book from requests page.",
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
                },
                cancel: function () {

                }
            }
        });


    }


    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    }
});