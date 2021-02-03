
myApp.controller('HomeCtrl', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm) {

    $scope.currUser = $('#currUserId').html();

    $scope.dashboardItems = {
        showDrawingOrderTree: false,
        showAvailableDrawingOrderTree: false,
        showBookBorrowTree: false,
        showArchivedBookBorrowTree: false,
        showAllBooks: false,
        showPendingRequests: false,
        grantedRequests: false,
        showPendingDrawingOrders:false

    }

   
    $scope.getCarTree = function () {
        $http.get(root + 'api/DrawingOrders/GetCarTree?drawingAvailable=false').then(function success(response) {
            $scope.carTree = response.data;
            $scope.carTreeBook = angular.copy($scope.carTree);
            console.log('tree:', $scope.carTree);
        }, function error() { });
    }

    $scope.getCarTreeAvailable = function () {
        $http.get(root + 'api/DrawingOrders/GetCarTree?drawingAvailable=true').then(function success(response) {
            $scope.carTreeAvailable = response.data;
            console.log('carTreeAvailable:', $scope.carTreeAvailable);
        }, function error() { });
    }

    $scope.getCarBookTree = function () {
        $http.get(root + 'api/DrawingOrders/GetCarBookTree?archive=false').then(function success(response) {
            $scope.carBookTree = response.data;
            console.log('book tree:', $scope.carBookTree);
        }, function error() { });
    }

    $scope.getCarArchivedBookTree = function () {
        $http.get(root + 'api/DrawingOrders/GetCarBookTree?archive=true').then(function success(response) {
            $scope.carArchivedBookTree = response.data;
            console.log('archive book tree:', $scope.carArchivedBookTree);
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

    $scope.getBooks = function () {
        $http.get(root + 'api/Books/GetBooks').then(function success(response) {
            $scope.allBooks = response.data;
            console.log('allBooks:', $scope.allBooks);
        }, function error() { });
    }

    $scope.getCarTree(); 
    $scope.getCarTreeAvailable();
    $scope.getCarBookTree();
    $scope.getCarArchivedBookTree();
    $scope.getPendingRequests();
    $scope.getGrantedRequests();
    $scope.getPendingDrawingOrders();
    $scope.getBooks();

    
    $scope.$watch('myCarTree.currentNode', function (newObj, oldObj) {
        if ($scope.abc && angular.isObject($scope.myCarTree.currentNode)) {
            console.log('Node Selected!!');
            console.log($scope.myCarTree.currentNode);
        }
    }, false);


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

    $scope.openBooksList = function (bookid) {
        var modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: root + 'Scripts/js/ng-templates/books-list-template.html',
            controller: 'BooksListCtrl',
            size: 'lg',
            resolve: {
                bookid: function () {
                    return bookid;
                }
            }
        });
    }

    $scope.openDrawingsList = function (orderid) {
        var modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: root + 'Scripts/js/ng-templates/drawings-list-template.html',
            controller: 'DrawingsListCtrl',
            size: 'lg',
            resolve: {
                orderid: function () {
                    return orderid;
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