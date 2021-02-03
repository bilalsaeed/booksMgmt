myApp.controller('DrawingsListCtrl', function ($scope, $filter, $http, $uibModalInstance, toaster, $ngConfirm, Lightbox, orderid) {
    
    $scope.orderid = orderid;

    $scope.getBookMediaList = function () {
        $http.get(root + 'api/DrawingOrders/GetDrawingFiles/' + $scope.orderid).then(function success(response) {
            $scope.files = response.data;
            //console.log('book files:', $scope.files);
        }, function error() { });
    }

    $scope.openLightboxModal = function (index) {
        Lightbox.openModal($scope.files, index);
    };

    $scope.getBookMediaList();

    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    }
});