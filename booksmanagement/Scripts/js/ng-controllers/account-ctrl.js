myApp.controller('AccountCtrl', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm) {

    $scope.deleteUser = function (userid) {
        $ngConfirm({
            title: 'Delete User?',
            content: 'Are you sure to delete this user? This action cannot be revert back',
            autoClose: 'cancel|10000',
            buttons: {
                submitRequest: {
                    text: 'Delete',
                    btnClass: 'btn-danger',
                    action: function () {
                        $http.post(root + 'Account/DeleteConfirmed/' + userid).then(function success(response) {
                            console.log(response);
                            if (response.status == 200) {
                                toaster.pop({
                                    type: 'success',
                                    title: '',
                                    body: response.statusText,
                                });
                                setTimeout(function () {
                                    window.location.reload();
                                }, 1000);
                            }
                            else {
                                toaster.pop({
                                    type: 'error',
                                    title: 'Error',
                                    body: response.statusText,
                                });
                            }
                        }, function error(err) {
                            console.log(err);
                            toaster.pop({
                                type: 'error',
                                title: 'Error',
                                body: err.statusText,
                            });
                        });
                    }
                },

                cancel: function () {

                }
            }
        });
    }

    $scope.yes = function () {
        alert('df');
    }

});