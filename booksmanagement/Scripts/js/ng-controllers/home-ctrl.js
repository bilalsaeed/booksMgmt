myApp.controller('HomeCtrl', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm) {

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


    $scope.getPendingRequests();
    $scope.getGrantedRequests();


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