<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Edument1.Default" Async="true" %>

<!doctype html>
<html lang="en">
<head>
    <!-- Required meta tags -->
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">



    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css" integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh" crossorigin="anonymous">


    <!-- Jquery -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js" integrity="sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo" crossorigin="anonymous"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js" integrity="sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6" crossorigin="anonymous"></script>

    <!-- Mattias CSS & JS -->
    <link rel="stylesheet" type="text/css" href="CSS/Global.css?debug=3">
    <script src="JS/Requests.js?debug=5"></script>
    <script src="JS/Filemanager.js?debug=5"></script>

    <!-- Font awesome for some icons -->
    <script src="https://kit.fontawesome.com/fad98e5e9a.js" crossorigin="anonymous"></script>


    <title>Mattias Filhanterare</title>
</head>
<body onload="initFileManager(defaults);">
    <div class="m-2">
        <div class="container">
            <div class="row border-bottom border-dark">
                <div class="col-12">
                    <h1>Mattias Filhanterare</h1>
                </div>
            </div>
            <i id="back_button" class="fas fa-long-arrow-left fa-2x cursor-pointer">Backa</i>
            <table class="table">

                <tbody align="center">
                    <tr>
                        <td id="add_button" data-toggle="modal" data-target="#exampleModal" class="cursor-pointer"><i class="fas fa-plus fa-2x"></i>Lägg till</td>
                        <td class="cursor-pointer"><i class="fas fa-book-open fa-2x"></i>Öppna(Ej gjort)</td>
                        <td id="remove_button" class="cursor-pointer"><i class="fas fa-trash fa-2x"></i>Radera</td>
                    </tr>
                </tbody>
            </table>
            <div class="row">
                <div class="col-2 border-right border-dark">
                </div>
                <div class="col-8">
                    <p runat="server" id="current_navigation" class="font-weight-bold">Nuvarande mapp</p>
                    <div align="center" id="center_view_container" class="row">
                    </div>
                </div>

            </div>
            <div class="row border-top border-dark">
                <div class="col-12">
                    <p>Skrivet i Javascript med Jquery/Ajax mot backend C#</p>
                </div>
            </div>
        </div>
        <footer>
            <div class="container bg-dark shadow">

                <p class="text-white">Edument projekt 2020-03-30</p>

            </div>
        </footer>

    </div>

    <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modal-title">Placeholder</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div id="modal-body" class="modal-body">
                </div>
                <div class="modal-footer" id="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Stäng</button>

                </div>
            </div>
        </div>
    </div>
</body>

<script>

    //Defaults from web.config
    var defaults = {
        requestBase: "<%=ConfigurationManager.AppSettings["requestBase"].ToString()%>",
        requestAdd: "<%=ConfigurationManager.AppSettings["requestAdd"].ToString() %>",
        requestRemove: "<%=ConfigurationManager.AppSettings["requestRemove"].ToString() %>",
        removeTextPart1: "<%=ConfigurationManager.AppSettings["removeTextPart1"].ToString() %>",
        removeTextPart2: "<%=ConfigurationManager.AppSettings["removeTextPart2"].ToString() %>",
        messageDeliveryOk: "<%=ConfigurationManager.AppSettings["messageDeliveryOk"].ToString() %>",
        messageDeliveryFailed: "<%=ConfigurationManager.AppSettings["messageDeliveryFailed"].ToString() %>",
        backError: "<%=ConfigurationManager.AppSettings["backError"].ToString() %>",
    }
</script>

</html>
