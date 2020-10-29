myApp.controller('BookBorrowCtrl', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm) {

    var modalInstance = null;
    //data api functions here

    $scope.getSoftBooks = function () {
        $http.get(root + 'api/Books/GetSoftBooks').then(function success(response) {
            $scope.softBooks = response.data;
            console.log('soft:',$scope.softBooks);
        }, function error() { });
    }

    $scope.getHardBooks = function () {
        $http.get(root + 'api/Books/GetHardBooks').then(function success(response) {
            $scope.hardBooks = response.data;

            const urlParams = new URLSearchParams(window.location.search);
            if (urlParams.get('bookId')) {
                if (urlParams.get('soft')) {

                }
                else {
                    var selectedBook;
                    var keepGoing = true;
                    angular.forEach($scope.hardBooks, function (item, key) {
                        if (keepGoing) {
                            //console.log('desc:' + item.CarPartComponentDescId, 'param:' + urlParams.get('desc'));
                            if (item.Id == urlParams.get('bookId')) {
                                selectedBook = item;
                                keepGoing = false;
                            }
                        }
                    });

                    $scope.openBorrowRequestModal(selectedBook);
                }
            }
            //else if (urlParams.get('comp')) {
            //    var selectedBook;
            //    var keepGoing = true;
            //    angular.forEach($scope.hardBooks, function (item, key) {
            //        if (keepGoing) {
            //            if (item.CarPartComponentId == urlParams.get('comp') && !item.CarPartComponentDescId) {
            //                selectedBook = item;
            //                keepGoing = false;
            //            }
            //        }
            //    });

            //    $scope.openBorrowRequestModal(selectedBook);
            //}
            //else if (urlParams.get('part')) {
            //    var selectedBook;
            //    var keepGoing = true;
            //    angular.forEach($scope.hardBooks, function (item, key) {
            //        if (keepGoing) {
            //            if (item.CarPartId == urlParams.get('part') && !item.CarPartComponentId) {
            //                selectedBook = item;
            //                keepGoing = false;
            //            }
            //        }
            //    });

            //    $scope.openBorrowRequestModal(selectedBook);
            //}
            //else if (urlParams.get('car')) {
            //    var selectedBook;
            //    var keepGoing = true;
            //    angular.forEach($scope.hardBooks, function (item, key) {
            //        if (keepGoing) {
            //            if (item.CarId == urlParams.get('car') && !item.CarPartId) {
            //                selectedBook = item;
            //                keepGoing = false;
            //            }
            //        }
            //    });

            //    $scope.openBorrowRequestModal(selectedBook);
            //}

            console.log('hard:',$scope.hardBooks);
        }, function error() { });
    }

    $scope.getSoftBooks();
    $scope.getHardBooks();


    //Other logical functions here

    $scope.openBorrowRequestModal = function (book) {
        var modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: root + 'Scripts/js/ng-templates/book-borrow-request-template.html',
            controller: 'BookBorrowRequestCtrl',
            size: 'lg',
            resolve: {
                book: function () {
                    return book;
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


myApp.controller('BookBorrowRequestCtrl', function ($scope, $filter, $http, $uibModalInstance, toaster, $ngConfirm, book) {
    $scope.book = book;


    $scope.saveBookRequest = function (bookData) {
        $http.post(root + 'api/BookBorrow/SaveBookRequest', bookData).then(function success(response) {
            if (response.status == 200) {
                toaster.pop({
                    type: 'success',
                    title: '',
                    body: "Request is submitted. You can check the status of request from your dashboard.",
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
        if ($scope.fromDate && $scope.toDate && $scope.purpose) {
            $ngConfirm({
                title: 'Submit Request?',
                content: 'Are you sure to submit this Request?',
                autoClose: 'cancel|10000',
                buttons: {
                    submitRequest: {
                        text: 'Submit',
                        btnClass: 'btn-success',
                        action: function () {
                            var bookData = {
                                FromDate: $scope.fromDate,
                                ToDate: $scope.toDate,
                                BookId: $scope.book.Id,
                                Purpose: $scope.purpose
                            } 
                            $scope.saveBookRequest(bookData);
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
                body: "Please fill the fields before submitting request.",
            });
        }
    }
    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    }
});