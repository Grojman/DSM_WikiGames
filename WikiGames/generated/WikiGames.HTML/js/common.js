/* Utilidades compartidas: iconos, barra de navegación, barra inferior y formateo. */
(function () {
  const icon = (paths, cls = "icon") =>
    `<svg class="${cls}" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true">${paths}</svg>`;

  const ICONS = {
    logo: `<svg class="logo" viewBox="0 0 32 32" aria-hidden="true"><rect x="1" y="1" width="30" height="30" fill="none" stroke="currentColor" stroke-width="1.5"/><path d="M9 13h4m-2-2v4M20 12.5h.01M23 15.5h.01" stroke="currentColor" stroke-width="2.2" stroke-linecap="round"/><path d="M8.5 9.5h15a3 3 0 0 1 3 3.2l-.6 7.3a2.5 2.5 0 0 1-4.3 1.5L19 19h-6l-2.6 2.5a2.5 2.5 0 0 1-4.3-1.5l-.6-7.3a3 3 0 0 1 3-3.2z" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linejoin="round"/></svg>`,
    settings: icon('<circle cx="12" cy="12" r="3"/><path d="M19.4 15a1.65 1.65 0 0 0 .33 1.82l.06.06a2 2 0 1 1-2.83 2.83l-.06-.06a1.65 1.65 0 0 0-1.82-.33 1.65 1.65 0 0 0-1 1.51V21a2 2 0 1 1-4 0v-.09A1.65 1.65 0 0 0 9 19.4a1.65 1.65 0 0 0-1.82.33l-.06.06a2 2 0 1 1-2.83-2.83l.06-.06A1.65 1.65 0 0 0 4.68 15a1.65 1.65 0 0 0-1.51-1H3a2 2 0 1 1 0-4h.09A1.65 1.65 0 0 0 4.6 9a1.65 1.65 0 0 0-.33-1.82l-.06-.06a2 2 0 1 1 2.83-2.83l.06.06A1.65 1.65 0 0 0 9 4.68a1.65 1.65 0 0 0 1-1.51V3a2 2 0 1 1 4 0v.09a1.65 1.65 0 0 0 1 1.51 1.65 1.65 0 0 0 1.82-.33l.06-.06a2 2 0 1 1 2.83 2.83l-.06.06A1.65 1.65 0 0 0 19.4 9a1.65 1.65 0 0 0 1.51 1H21a2 2 0 1 1 0 4h-.09a1.65 1.65 0 0 0-1.51 1z"/>'),
    login: icon('<path d="M15 3h4a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2h-4"/><path d="M10 17l5-5-5-5"/><path d="M15 12H3"/>'),
    search: icon('<circle cx="11" cy="11" r="7"/><path d="M21 21l-4.3-4.3"/>'),
    user: icon('<circle cx="12" cy="8" r="4"/><path d="M4 21a8 8 0 0 1 16 0"/>'),
    plus: icon('<path d="M12 5v14M5 12h14"/>'),
    heart: icon('<path d="M20.8 4.6a5.5 5.5 0 0 0-7.8 0L12 5.7l-1-1.1a5.5 5.5 0 0 0-7.8 7.8l1 1.1L12 21l7.8-7.5 1-1.1a5.5 5.5 0 0 0 0-7.8z"/>'),
    trash: icon('<path d="M3 6h18M8 6V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2M19 6l-1 14a2 2 0 0 1-2 2H8a2 2 0 0 1-2-2L5 6"/>'),
    check: icon('<path d="M20 6L9 17l-5-5"/>'),
    x: icon('<path d="M18 6L6 18M6 6l12 12"/>'),
    flag: icon('<path d="M4 15s1-1 4-1 5 2 8 2 4-1 4-1V3s-1 1-4 1-5-2-8-2-4 1-4 1z"/><path d="M4 22v-7"/>'),
    gamepad: icon('<path d="M6 11h4M8 9v4M15 12h.01M18 10h.01"/><rect x="2" y="6" width="20" height="12" rx="4"/>'),
    image: icon('<rect x="3" y="3" width="18" height="18" rx="2"/><circle cx="8.5" cy="8.5" r="1.5"/><path d="M21 15l-5-5L5 21"/>'),
    back: icon('<path d="M15 18l-6-6 6-6"/>'),
  };

  const STAR = '<svg viewBox="0 0 24 24" aria-hidden="true"><path d="M12 2.5l2.9 6 6.6.9-4.8 4.6 1.2 6.5L12 17.4l-5.9 3.1 1.2-6.5L2.5 9.4l6.6-.9z"/></svg>';

  const esc = (s) => String(s ?? "").replace(/[&<>"']/g, (c) => ({ "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#39;" }[c]));

  const fmtDate = (d) => new Date(d).toLocaleDateString("es-ES", { day: "2-digit", month: "short", year: "numeric" });
  const fmtDateTime = (d) => new Date(d).toLocaleString("es-ES", { day: "2-digit", month: "short", year: "numeric", hour: "2-digit", minute: "2-digit" });

  /** Puntuación 0–10 del modelo → estrellas 0–5 llenas (redondeo a la estrella más cercana). */
  const starsHtml = (puntuacion) => {
    const filled = Math.round(puntuacion / 2);
    let html = `<span class="stars" role="img" aria-label="${filled} de 5 estrellas">`;
    for (let i = 1; i <= 5; i++) html += `<span class="star ${i <= filled ? "on" : ""}">${STAR}</span>`;
    return html + "</span>";
  };

  const user = (id) => WG.usuarios.find((u) => u.id === id) || { id, name: "usuario eliminado" };

  /** Avatar con la inicial del usuario y un tono derivado de su id. */
  const avatar = (u, size = "") => `<span class="avatar ${size}">${esc(u.name[0].toUpperCase())}</span>`;

  /** Imagen por defecto del videojuego; si no tiene o falla, muestra un placeholder con su título. */
  const cover = (v) => {
    const ph = `<div class="cover-ph"><span>${esc(v.titulo)}</span></div>`;
    if (!v.imagen) return ph;
    return `<img src="${esc(v.imagen)}" alt="${esc(v.titulo)}" loading="lazy" onerror="this.outerHTML=this.nextElementSibling.innerHTML"><template>${ph}</template>`;
  };

  const params = new URLSearchParams(location.search);
  const currentGame = () => {
    const id = Number(params.get("juego"));
    return WG.videojuegos.find((v) => v.id === id) || WG.videojuegos[0];
  };

  function renderNavbar() {
    const el = document.querySelector("[data-navbar]");
    if (!el) return;
    el.className = "navbar";
    el.innerHTML = `
      <a class="brand" href="index.html">${ICONS.logo}<span>WikiGames</span></a>
      <nav class="nav-actions" aria-label="Cuenta">
        <a class="icon-btn" href="#" title="Ajustes" aria-label="Ajustes">${ICONS.settings}</a>
        <a class="btn btn-ghost" href="#">${ICONS.login}<span>Iniciar sesión</span></a>
      </nav>`;
  }

  function renderBottomBar() {
    const el = document.querySelector("[data-bottombar]");
    if (!el) return;
    const page = document.body.dataset.page;
    el.className = "bottombar";
    el.setAttribute("aria-label", "Accesos rápidos");
    el.innerHTML = `
      <a href="#" class="bb-item ${page === "perfil" ? "active" : ""}" title="Mi perfil">${ICONS.user}<span>Perfil</span></a>
      <a href="crear-videojuego.html" class="bb-item bb-primary ${page === "crear" ? "active" : ""}" title="Publicar videojuego">${ICONS.plus}<span>Publicar</span></a>
      <a href="#" class="bb-item ${page === "favoritos" ? "active" : ""}" title="Favoritos">${ICONS.heart}<span>Favoritos</span></a>`;
  }

  /** Cabecera del foro de un videojuego, con pestañas Conversaciones / Comentarios. */
  function renderGameHeader(active) {
    const el = document.querySelector("[data-game-header]");
    if (!el) return;
    const v = currentGame();
    const cats = v.categorias.map((id) => WG.categorias.find((c) => c.id === id)).filter(Boolean);
    el.className = "game-header";
    el.innerHTML = `
      <div class="game-header-cover">${cover(v)}</div>
      <div class="game-header-info">
        <a class="back-link" href="index.html">${ICONS.back}Inicio</a>
        <h1>${esc(v.titulo)}</h1>
        <p class="muted">${esc(v.texto)}</p>
        <div class="chips">${cats.map((c) => `<span class="chip">${esc(c.name)}</span>`).join("")}</div>
      </div>
      <nav class="tabs" aria-label="Secciones del foro">
        <a href="conversaciones.html?juego=${v.id}" class="${active === "conversaciones" ? "active" : ""}">Conversaciones</a>
        <a href="comentarios.html?juego=${v.id}" class="${active === "comentarios" ? "active" : ""}">Comentarios</a>
      </nav>`;
  }

  function toast(msg) {
    let t = document.querySelector(".toast");
    if (!t) {
      t = document.createElement("div");
      t.className = "toast";
      t.setAttribute("role", "status");
      document.body.appendChild(t);
    }
    t.textContent = msg;
    t.classList.add("show");
    clearTimeout(t._h);
    t._h = setTimeout(() => t.classList.remove("show"), 2600);
  }

  window.UI = { ICONS, STAR, esc, fmtDate, fmtDateTime, starsHtml, user, avatar, cover, params, currentGame, renderGameHeader, toast };

  document.addEventListener("DOMContentLoaded", () => {
    renderNavbar();
    renderBottomBar();
    document.querySelectorAll("[data-icon]").forEach((n) => (n.innerHTML = ICONS[n.dataset.icon] || ""));
  });
})();
