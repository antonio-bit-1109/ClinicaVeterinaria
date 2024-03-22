const button_findAnimalbyMicroChip = document.getElementById("findAnimalbyMicroChip");
const valoreMicrochip = document.getElementById("inputInserisciNumMicro");
let Animalecercato = {};
const containerBello = document.getElementById("containerBello");

const FetchanimaleRicoverato = (id) => {
    fetch(`/animali/IsAnimaleRicoverato/?idAnimale=${id}`)
        .then(response => response.json())
        .then(data => {
            console.log(data);
            let p = document.createElement('p');
            if (data !== null) {

                p.textContent = "Il tuo animale è ricoverato da noi";

            } else {
                p.textContent = "Il tuo animale non è ricoverato da noi";
            } 
            containerBello.appendChild(p);
        })
}

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

            // Creazione del nodo principale 'div' con classe 'card'
            let cardDiv = document.createElement('div');
            cardDiv.className = 'card';

            // Creazione dell'elemento 'img'
            let img = document.createElement('img');
            img.style.height = '100px';
            img.style.objectFit = 'cover';
            img.src = `../images/animali/${Animalecercato.fotoAnimale}`;
            img.className = 'card-img-top';
            img.alt = '...';
            cardDiv.appendChild(img);

            // Creazione del nodo 'div' per il corpo della card
            let cardBody = document.createElement('div');
            cardBody.className = 'card-body';
            cardDiv.appendChild(cardBody);

            // Creazione degli elementi 'h5' e 'p'
            let h5 = document.createElement('h5');
            h5.className = 'card-title';
            h5.textContent = Animalecercato.nomeAnimale;
            cardBody.appendChild(h5);

            let h6 = document.createElement('p');
            h6.className = 'card-title';
            h6.textContent = Animalecercato.coloreMantello;
            cardBody.appendChild(h6);

            let h7 = document.createElement('p');
            h7.className = 'card-title';
            h7.textContent = Animalecercato.dataregistrazione;
            cardBody.appendChild(h7);


            let h8 = document.createElement('p');
            h8.className = 'card-title';
            h8.textContent = Animalecercato.numMicrochip;
            cardBody.appendChild(h8);

            let pId = document.createElement('p');
            pId.className = 'card-text';
            pId.textContent = Animalecercato.idAnimale;
            cardBody.appendChild(pId);

            // ... Aggiungi qui gli altri elementi 'p' ...

            // Creazione del bottone
            let button = document.createElement('button');
            button.id = 'ricoveroButton';
            button.className = 'btn btn-primary';
            button.textContent = 'Il tuo animale è ricoverato da noi ?';
            cardBody.appendChild(button);

            // Aggiunta dell'evento click al bottone

            // Aggiunta della card al container
            containerBello.appendChild(cardDiv);

            button.addEventListener('click', () => {
                FetchanimaleRicoverato(Animalecercato.idAnimale);
            });
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