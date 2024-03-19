const inputHasMicrochip = document.getElementById("inputHasMicrochip");
const divNumMicrochip = document.getElementById("divNumMicrochip");
const inputHasProprietario = document.getElementById("inputHasProprietario");
const divIdUtente = document.getElementById("divIdUtente");

const mostraNascondi = (input, div) => {
    if (!input.checked) {
        div.style.display = "none";
    }

    input.addEventListener('change', () => {
        if (!input.checked) {
            div.style.display = "none";
        } else {
            div.style.display = "block";
        }
    });
}


document.addEventListener('DOMContentLoaded', () => {

    mostraNascondi(inputHasMicrochip, divNumMicrochip);
    mostraNascondi(inputHasProprietario, divIdUtente);

});