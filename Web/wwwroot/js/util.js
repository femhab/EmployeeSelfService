function alerty(type, header = "", msg = "") {
    $("#content").prepend(
        `<div class="alerty alert alert-${type} fade hide " id="bsalert">
                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                <strong>${header}</strong> ${msg}
            </div>`
    );
    $("#bsalert").removeClass("hide");
    $("#bsalert").addClass("show");
    setTimeout(function () {
        $('.alerty .close').click();
    }, 3000)
}