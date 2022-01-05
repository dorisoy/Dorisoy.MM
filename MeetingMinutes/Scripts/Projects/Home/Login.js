app.controller("cLogin", ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {

    //catch access token and user name    
    var href = window.location.href;
    $scope.background = href + "/Content/assets/img/bg.jpg";
    if (href.endsWith("/"))
        $scope.background = "Content/assets/img/bg.jpg";

    var tokenKey = 'accessToken';
    $scope.data = {};

    $scope.clearData = function () {
        $scope.data = {};
        $scope.detailsData = {};
    };
    
    //login data 
    $scope.login = function (data) {

        $scope.$broadcast('show-errors-event');

        if ($scope.loginForm.$invalid) {
            swal(
                '表单缺失',
                '请输入必要数据.',
                'error'
              );
            return;
        }
        document.body.style.cursor = 'wait';
        $http({
            url: window.location.href + "/token",
            method: "POST",
            data: "username=" + data.Email + "&password=" + data.Password +
              "&grant_type=password",
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).then(function (response) {
            document.body.style.cursor = 'default';
            // Cache the access token in session storage.
            sessionStorage.setItem(tokenKey, response.data.access_token);
            //var path = window.location.pathname + "/Home/Meeting";
            window.location.href = response.data.path + "Home/Meeting";

        }, function (response) {
            document.body.style.cursor = 'default';
            var obj = response.data;
            //alert(obj.error);
            if (obj.error_description == "Account pending approval.") {
                swal(
                    '待核准帐户',
                    '您的帐户正在等待批准，请验证您的电子邮件地址',
                    'error'
                  );
            }
            else if (obj.error_description == "The user name or password is incorrect.") {
                swal(
                    '对不起!',
                    '用户名或密码不正确.',
                    'error'
                  );
            }
        });
    };

    


    //Recover Password data start
    $scope.RecoverPassword = function (data) {
        // alert("save data call " + data.Email);
        $scope.$broadcast('show-errors-event');
        if ($scope.newForm.$invalid) {
            swal(
                '表单缺失',
                '请输入必要数据.',
                'error'
              );
            return;
        }

        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        url = '../api/Logout/ForgotPasswordSendMail';
        document.body.style.cursor = 'wait';
        $http({
            method: 'POST',
            url: url,
            data: data,
            headers: headers,
        }).then(function (response) {
            if (response.status == 200) {
                document.body.style.cursor = 'default';
                swal(
                       'Successfully Mail Sent',
                       'Please check your email address.',
                       'success'
                     ).then((value) => {
                         document.body.style.cursor = 'wait';
                         window.location.href = response.data;
                     });
                $scope.clearData();
            }
        }, function (response) {
            var obj = response.data;
            if (obj == "invalid email") {
                swal(
                    'Error',
                    'Could not find your email id',
                    'error'
                  );
            }
        });
    };
    //Recover Password data End


    //forgot password start
    $scope.ChangePassword = function (data) {
        //alert(data.NewPassword);

        $scope.$broadcast('show-errors-event');

        if ($scope.newForm.$invalid) {
            swal(
                '表单缺失',
                '请输入必要数据.',
                'error'
              );
            return;
        }

        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        //catch userid & code for change password from url
        var userId = null, tmp = [], code = null;
        var items = window.location.search.substr(1).split("&");
        for (var index = 0; index < items.length; index++) {
            tmp = items[index];
            //   alert(tmp + " ok ");
            if (index == 0) {
                userId = decodeURIComponent(tmp);
                var userId = userId.substring(7, userId.length);
                //alert(userId + " userId");
            }
            if (index == 1) {
                code = decodeURIComponent(tmp);
                var code = code.substring(5, code.length);
                // alert(code + " code");
            }
        }
        //set user id and code data
        data.userId = userId;
        data.code = code;
        url = '../api/Logout/SetPassword';
        document.body.style.cursor = 'wait';
        $http({
            method: 'POST',
            url: url,
            data: data,
            headers: headers,
        }).then(function (response) {
            if (response.status == 200) {
                document.body.style.cursor = 'wait';
                window.location.href = response.data;
            }
        }, function (response) {
            document.body.style.cursor = 'default';
            swal(
               'Error',
               '请输入必要数据.',
               'error'
             );
        });
    }
    //forgot password End

}]);
