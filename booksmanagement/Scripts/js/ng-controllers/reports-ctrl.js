myApp.controller('ReportsCtrl', function ($scope, $filter, $http, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {

    $scope.vm = {};
    $scope.vmBooks = {};
    $scope.vmDrawings = {};

    $scope.vm.dtInstance = {};
    $scope.vmBooks.dtInstance = {};
    $scope.vmDrawings.dtInstance = {};

    $scope.vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('paging', true)
        .withOption('searching', true)
        .withOption('info', true)
        .withButtons([
            //{
            //    extend: 'copy',
            //    text: '<i class="fa fa-files-o"></i> Copy',
            //    titleAttr: 'Copy'
            //},
            {
                extend: 'print',
                text: '<i class="fa fa-print" aria-hidden="true"></i> Print',
                titleAttr: 'Print',
                title: function () {
                    return "<center><h3>Books borrowed report</h3></center><br />"
                }
            },
            //{
            //    extend: 'excel',
            //    text: '<i class="fa fa-file-text-o"></i> Excel',
            //    titleAttr: 'Excel'
            //}
        ]);

    $scope.vmBooks.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('paging', true)
        .withOption('searching', true)
        .withOption('info', true)
        .withButtons([
            //{
            //    extend: 'copy',
            //    text: '<i class="fa fa-files-o"></i> Copy',
            //    titleAttr: 'Copy'
            //},
            {
                extend: 'print',
                text: '<i class="fa fa-print" aria-hidden="true"></i> Print',
                titleAttr: 'Print',
                title: function () {
                    return "<center><h3>Books addition report</h3></center><br />"
                }
            },
            //{
            //    extend: 'excel',
            //    text: '<i class="fa fa-file-text-o"></i> Excel',
            //    titleAttr: 'Excel'
            //}
        ]);

    $scope.vmDrawings.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('paging', true)
        .withOption('searching', true)
        .withOption('info', true)
        .withButtons([
            //{
            //    extend: 'copy',
            //    text: '<i class="fa fa-files-o"></i> Copy',
            //    titleAttr: 'Copy'
            //},
            {
                extend: 'print',
                text: '<i class="fa fa-print" aria-hidden="true"></i> Print',
                titleAttr: 'Print',
                title: function () {
                    return "<center><h3>Drawings report</h3></center><br />"
                }
            },
            //{
            //    extend: 'excel',
            //    text: '<i class="fa fa-file-text-o"></i> Excel',
            //    titleAttr: 'Excel'
            //}
        ]);


    $scope.searchQuery = {
        FromDate: null,
        ToDate: null,
        ExceedDueDate: false
    }

    $scope.getAllRequests = function () {
        $http.post(root + 'api/BookBorrow/SearchAllRequests', $scope.searchQuery).then(function success(response) {
            $scope.allRequests = response.data;
            console.log('allRequests:', $scope.allRequests);
        }, function error() { });
    }

    $scope.getAllBooks = function () {
        $http.post(root + 'api/Books/SearchBooks', $scope.searchQuery).then(function success(response) {
            $scope.allBooks = response.data;
            console.log('allBooks:', $scope.allBooks);
        }, function error() { });
    }

    $scope.getAllDrawings = function () {
        $http.post(root + 'api/DrawingOrders/SearchAllDrawings', $scope.searchQuery).then(function success(response) {
            $scope.allDrawings = response.data;
            console.log('allDrawings:', $scope.allDrawings);
        }, function error() { });
    }


    $scope.getAllRequests();
    $scope.getAllBooks();
    $scope.getAllDrawings();


    //Other functions here
    $scope.clearFilters = function () {
        $scope.searchQuery = {
            FromDate: null,
            ToDate: null,
            ExceedDueDate: false
        }
    }

});