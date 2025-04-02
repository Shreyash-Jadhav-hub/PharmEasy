<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Admin_Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>

    <!-- Bootstrap and jQuery -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.4/dist/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>

    <style>
        :root {
            --primary-color: #007bff;
            --primary-color-hover: #0056b3;
            --border-radius: 10px;
            --box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            --border-color: #ccc;
            --input-padding: 10px;
            --container-width: 400px;
            --margin-bottom: 36px;
        }

        body {
            font-family: Arial, sans-serif;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            margin: 0;
            background-image: url('img/bg4.jpeg');
            background-size: 90%;
            background-position: center;
            background-repeat: no-repeat;
            position: relative;
        }

        .login-container {
            position: absolute;
            top: 59%;
            left: 50.5%;
            transform: translate(-50%, -50%);
            width: 480px; /* Adjust the width as needed */
            padding: 30px;
            border: 1px solid var(--border-color);
            border-radius: var(--border-radius);
            box-shadow: var(--box-shadow);
            background-color: #E0E0E0;
        }

        .form-group {
            margin-bottom: var(--margin-bottom);
        }

        .form-control {
            padding: var(--input-padding);
            border: 1px solid var(--border-color);
            border-radius: var(--border-radius);
        }

        .btn {
            width: 60%;
            padding: var(--input-padding);
            border: none;
            border-radius: var(--border-radius);
            background-color: var(--primary-color);
            color: #fff;
            cursor: pointer;
            transition: background-color 0.3s;
        }

        .btn-primary:hover {
            background-color: var(--primary-color-hover);
        }

        .button-container {
            display: flex;
            justify-content: center;
        }

        .login-container h2 {
            text-align: center;
            margin-bottom: var(--margin-bottom);
        }

        .error-message {
            color: red;
            text-align: center;
            margin-bottom: var(--margin-bottom);
        }

        /* Custom styles for label and input alignment */
        .form-group .col-form-label {
            text-align: right;
            padding-right: 55px;
        }

        .form-group .form-control {
            flex: 1;
        }

    .boxed-heading {
    display: flex;
    justify-content: left;
    padding: 5px;
    background: none; /* Remove background */
    color: #333; /* Darker text color for contrast */
    border: none; /* Remove border */
    border-radius: 0; /* Remove rounded corners */
    box-shadow: none; /* Remove shadow */
    font-weight: bold;
    font-size: 34px; 
}

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <h3 class="boxed-heading">Login</h3>
            <asp:Label ID="txtUser" runat="server" CssClass="error-message" />
            <br />
            <div class="form-group row">
                <label for="username" class="col-sm-4 col-form-label">Username:</label>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter Username"/>
                </div>
            </div>

            <div class="form-group row">
                <label for="password" class="col-sm-4 col-form-label">Password:</label>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Enter Password" />
                </div>
            </div>

            <div class="button-container">
                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary" OnClick="btnLogin_Click" />
            </div>
        </div>
    </form>
</body>
</html>
