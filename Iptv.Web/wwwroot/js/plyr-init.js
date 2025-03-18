window.initializePlyr = () => {
    const player = document.querySelector("#player");

    if (!player) {
        console.error("Elemento #player não encontrado.");
        return; // Retorna e não tenta fazer nada se o player não for encontrado
    }

    // Se a imagem do poster estiver definida com data-poster, usamos ela
    const poster = player.getAttribute("data-poster");
    if (poster) {
        player.poster = poster;
    }

    // Inicializa o Plyr com as configurações desejadas
    const plyr = new Plyr(player, {
        controls: ['play', 'progress', 'current-time', 'mute', 'volume', 'fullscreen'],
    });

    player.style.width = "100%";
};

window.addEventListener("DOMContentLoaded", (event) => {
    // Inicializa o Plyr quando o DOM estiver completamente carregado
    initializePlyr();
});