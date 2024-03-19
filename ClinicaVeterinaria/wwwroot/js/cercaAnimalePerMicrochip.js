const button_findAnimalbyMicroChip = document.getElementById("findAnimalbyMicroChip");
const valoreMicrochip = document.getElementById("inputInserisciNumMicro");
let Animalecercato = {};


const myFetch = (valore) => {

    options = {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    }


    fetch(`/animali/getAnimalByMicrochip/?stringMicro=${valore}`, options)
        .then(response => response.json())
        .then(data => {
            Animalecercato = data;
            console.log(Animalecercato);

            let containerBello = document.getElementById("containerBello");

            containerBello.innerHTML =
                `
				<div class="card">
                    <img style="height:100px; object-fit:cover" src="../images/animali/${Animalecercato.fotoAnimale}" class="card-img-top" alt="...">
                  <div class="card-body">
                    <h5 class="card-title">${Animalecercato.nomeAnimale}</h5>
                    <p class="card-text">${Animalecercato.idanimale}</p>
                   <p class="card-text">${Animalecercato.coloreMantello}</p>
                   <p class="card-text">${Animalecercato.dataregistrazione}</p>
                   <p class="card-text">${Animalecercato.hasMicrochip}</p>
                   <p class="card-text">${Animalecercato.hasProprietario}</p>
                   <p class="card-text">${Animalecercato.numMicrochip}</p>
                   <p class="card-text">${Animalecercato.tipologia}</p>
                  </div>
                </div>
				`
        })

}



document.addEventListener("DOMContentLoaded", () => {

    button_findAnimalbyMicroChip.addEventListener("click", () => {

        let valore = valoreMicrochip.value;
        if (valore !== "") {

            myFetch(valore)
        }
        else {
            let infoOutput = document.getElementById("infoOutput");
            infoOutput.innerHTML = "Inserisci un valore valido";
        }
    })
})