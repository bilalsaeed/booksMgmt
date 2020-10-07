myApp.controller('HomeCtrl', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm) {

    $scope.getPendingRequests = function () {
        $http.get(root + 'api/BookBorrow/GetPendingRequests').then(function success(response) {
            $scope.pendingRequests = response.data;
            console.log('requests:', $scope.pendingRequests);
        }, function error() { });
    }

    $scope.getPendingRequests();
});