myApp.controller('BooksListCtrl', function ($scope, $filter, $http, $uibModalInstance, toaster, $ngConfirm, bookid) {
    
    $scope.bookid = bookid;

    $scope.getBookMediaList = function(){
        $http.get(root + 'api/Books/GetBookSoftFiles/' + $scope.bookid).then(function success(response) {
            $scope.files = response.data;
            //console.log('book files:', $scope.files);
        }, function error() { });
    }

    $scope.getBookMediaList();

    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    }
});