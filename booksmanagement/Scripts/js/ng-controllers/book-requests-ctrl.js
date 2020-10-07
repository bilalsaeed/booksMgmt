myApp.controller('BookRequestsCtrl', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm) {

    var vm = this;

    $scope.getAllBookRequests = function () {
        $http.get(root + 'api/BookBorrow/GetAllRequests').then(function success(response) {
            $scope.allRequests = response.data;
            console.log('all requests:', $scope.allRequests);
            
        }, function error() { });
    }

    $scope.grantBook = function (requestId) {
        $ngConfirm({
            title: 'Grant this Book?',
            content: 'Are you sure to grant this book?',
            autoClose: 'cancel|10000',
            buttons: {
                submitRequest: {
                    text: 'Yes',
                    btnClass: 'btn-success',
                    action: function () {
                        $http.post(root + 'api/BookBorrow/GrantBook', requestId).then(function success(response) {
                            if (response.status == 200) {
                                toaster.pop({
                                    type: 'success',
                                    title: '',
                                    body: "Book is granted.",
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

    $scope.alreadyGotBook = function (requestId) {
        $ngConfirm({
            title: 'Have you got this Book?',
            content: 'Are you sure you have got this book?',
            autoClose: 'cancel|10000',
            buttons: {
                submitRequest: {
                    text: 'Yes',
                    btnClass: 'btn-success',
                    action: function () {
                        $http.post(root + 'api/BookBorrow/GrantBook', requestId).then(function success(response) {
                            if (response.status == 200) {
                                toaster.pop({
                                    type: 'success',
                                    title: '',
                                    body: "We have recorded this in our system. Thank you for cooperation",
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


    $scope.getAllBookRequests();

});