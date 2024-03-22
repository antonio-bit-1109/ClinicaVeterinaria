document.addEventListener("DOMContentLoaded", function () {
    var cronologiaVisiteModal = new bootstrap.Modal(document.getElementById("cronologia-visite-modal"));

    document.querySelectorAll(".cronologia-visite-btn").forEach(btn => {
        btn.addEventListener("click", function () {
            var animaleId = this.getAttribute("data-animale-id");

            fetch(`/Visite/CronistoriaVisite?id=${animaleId}`)
                .then(response => response.json())
                .then(data => {
                    var container = document.getElementById("cronologia-visite");
                    container.innerHTML = "";

                    data.forEach(visita => {
                        var visitaElement = document.createElement("div");
                        visitaElement.classList.add("col-12", "mb-3");

                        var visitaCard = `
                                            <div class="card">
                                                <div class="card-body">
                                                    <h5 class="card-title">Visita del ${new Date(visita.dataVisita).toLocaleDateString()}</h5>
                                                    <p class="card-text">Anamnesi: ${visita.anamnesi}</p>
                                                    <p class="card-text">Descrizione Cura: ${visita.descrizioneCura}</p>
                                                </div>
                                            </div>
                                        `;

                        visitaElement.innerHTML = visitaCard;
                        container.appendChild(visitaElement);
                    });

                    cronologiaVisiteModal.show();
                })
                .catch(error => console.error("Errore durante il recupero della cronologia delle visite:", error));
        });
    });
});