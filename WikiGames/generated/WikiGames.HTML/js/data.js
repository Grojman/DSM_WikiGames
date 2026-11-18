/*
 * Datos de prueba para las maquetas HTML.
 * Reflejan el modelo del TDM (WikiGames.puml) y los datos de InitializeDB/SeedData.cs.
 * Cuando las páginas se integren en WikiGames.Web, estos datos vendrán de los controladores.
 */
(function () {
  const now = new Date();
  const daysAgo = (d, h = 0) => new Date(now.getTime() - (d * 24 + h) * 3600 * 1000);

  // Rol: Registrado | Moderador | Administrador
  const usuarios = [
    { id: 1, name: "admin", rolUsuario: "Administrador" },
    { id: 2, name: "moderador", rolUsuario: "Moderador" },
    { id: 3, name: "ana", rolUsuario: "Registrado" },
    { id: 4, name: "pablo", rolUsuario: "Registrado" },
    { id: 5, name: "lucia", rolUsuario: "Registrado" },
    { id: 6, name: "marcos_dev", rolUsuario: "Registrado" },
    { id: 7, name: "irene.games", rolUsuario: "Registrado" },
  ];

  const categorias = [
    { id: 1, name: "Acción", padre: null },
    { id: 2, name: "Aventura", padre: null },
    { id: 3, name: "RPG", padre: null },
    { id: 4, name: "Estrategia", padre: null },
    { id: 5, name: "Shooter", padre: 1 },
    { id: 6, name: "Plataformas", padre: 1 },
    { id: 7, name: "RPG de acción", padre: 3 },
    { id: 8, name: "Miedo", padre: null },
    { id: 9, name: "Puzzle", padre: null },
    { id: 10, name: "Simulación", padre: null },
  ];

  // EstadoContenido: Pendiente | Aprobado | Rechazado
  const videojuegos = [
    { id: 10, titulo: "Starfall Odyssey", texto: "Exploración espacial con combates por turnos.", enlace: "https://example.com/starfall", imagen: "", estado: "Aprobado", numeroMeGusta: 3, puntuacionMedia: 8.75, fechaCreacion: daysAgo(60), usuarioCreador: 3, categorias: [3, 2] },
    { id: 11, titulo: "Neon Rush", texto: "Shooter arcade frenético en una ciudad cyberpunk.", enlace: "https://example.com/neonrush", imagen: "", estado: "Aprobado", numeroMeGusta: 2, puntuacionMedia: 5.75, fechaCreacion: daysAgo(30), usuarioCreador: 4, categorias: [1, 5] },
    { id: 12, titulo: "Kingdoms of Ash", texto: "Estrategia en tiempo real con gestión de reinos.", enlace: "https://example.com/kingdoms", imagen: "", estado: "Aprobado", numeroMeGusta: 1, puntuacionMedia: 4.25, fechaCreacion: daysAgo(15), usuarioCreador: 5, categorias: [4] },
    { id: 13, titulo: "Pixel Jump", texto: "Plataformas 2D de precisión.", enlace: "https://example.com/pixeljump", imagen: "", estado: "Pendiente", numeroMeGusta: 0, puntuacionMedia: 0, fechaCreacion: daysAgo(2), usuarioCreador: 3, categorias: [6] },
    { id: 14, titulo: "Copia Barata", texto: "Juego sin información relevante.", enlace: "https://example.com/copia", imagen: "", estado: "Rechazado", numeroMeGusta: 0, puntuacionMedia: 0, fechaCreacion: daysAgo(5), usuarioCreador: 4, categorias: [1] },
    { id: 15, titulo: "Hollow Lantern", texto: "Terror psicológico en una mansión que cambia cada noche.", enlace: "", imagen: "", estado: "Aprobado", numeroMeGusta: 7, puntuacionMedia: 7.9, fechaCreacion: daysAgo(9), usuarioCreador: 6, categorias: [8, 2] },
    { id: 16, titulo: "Gearbound", texto: "Puzles mecánicos con física de engranajes.", enlace: "", imagen: "", estado: "Aprobado", numeroMeGusta: 4, puntuacionMedia: 9.1, fechaCreacion: daysAgo(4), usuarioCreador: 7, categorias: [9] },
    { id: 17, titulo: "Tidefarm", texto: "Simulador de granja en una isla con mareas.", enlace: "", imagen: "", estado: "Aprobado", numeroMeGusta: 5, puntuacionMedia: 6.4, fechaCreacion: daysAgo(1), usuarioCreador: 6, categorias: [10] },
    { id: 18, titulo: "Ember Knights", texto: "RPG de acción cooperativo con mazmorras procedurales.", enlace: "", imagen: "", estado: "Aprobado", numeroMeGusta: 9, puntuacionMedia: 8.2, fechaCreacion: daysAgo(21), usuarioCreador: 7, categorias: [7, 1] },
  ];

  // Valoracion.puntuacion es float 0–10 (ver SeedData). En la UI: 5 estrellas = 10 puntos.
  const valoraciones = [
    { id: 30, videojuego: 10, usuarioCreador: 4, puntuacion: 9.5, texto: "Una obra maestra del género.", fechaCreacion: daysAgo(50) },
    { id: 31, videojuego: 10, usuarioCreador: 5, puntuacion: 8.0, texto: "Muy bueno, aunque algo largo. La parte central se hace pesada, pero el tramo final compensa con creces: la banda sonora y el diseño de los combates por turnos están a un nivel que no esperaba en una demo.", fechaCreacion: daysAgo(40) },
    { id: 32, videojuego: 11, usuarioCreador: 3, puntuacion: 7.0, texto: "Divertido para partidas cortas.", fechaCreacion: daysAgo(20) },
    { id: 33, videojuego: 11, usuarioCreador: 5, puntuacion: 4.5, texto: "Repetitivo después de unas horas.", fechaCreacion: daysAgo(18) },
    { id: 34, videojuego: 12, usuarioCreador: 4, puntuacion: 6.5, texto: "Buena idea, mala interfaz.", fechaCreacion: daysAgo(10) },
    { id: 35, videojuego: 12, usuarioCreador: 3, puntuacion: 2.0, texto: "No me ha gustado nada.", fechaCreacion: daysAgo(8) },
  ];

  const temas = [
    { id: 40, videojuego: 10, usuarioCreador: 5, titulo: "Builds de Starfall", texto: "¿Cuál es la mejor build para el capitán?", fechaCreacion: daysAgo(35) },
    { id: 41, videojuego: 11, usuarioCreador: 4, titulo: "Récords de Neon Rush", texto: "Compartid vuestras mejores puntuaciones.", fechaCreacion: daysAgo(12) },
    { id: 42, videojuego: 12, usuarioCreador: 3, titulo: "Guía de inicio", texto: "Guía para principiantes de Kingdoms of Ash.", fechaCreacion: daysAgo(3) },
    { id: 43, videojuego: 10, usuarioCreador: 3, titulo: "Fallo al guardar en el sector 4", texto: "Cuando entro en la estación del sector 4 y guardo la partida, al recargar aparezco fuera del mapa. Me ha pasado tres veces seguidas con la versión 0.3 de la demo. ¿A alguien más le ocurre? Adjunto los pasos exactos para reproducirlo por si el equipo lo lee.", fechaCreacion: daysAgo(6) },
    { id: 44, videojuego: 10, usuarioCreador: 4, titulo: "Ideas para la versión completa", texto: "Abro este hilo para recopilar sugerencias: más tripulantes, misiones secundarias y un modo foto.", fechaCreacion: daysAgo(1, 4) },
  ];

  const comentarios = [
    { id: 50, tema: 40, usuarioCreador: 4, texto: "Yo voy full ingeniería, va genial." },
    { id: 51, tema: 40, usuarioCreador: 5, texto: "¡Gracias! Lo probaré esta noche." },
    { id: 52, tema: 40, usuarioCreador: 3, texto: "La ingeniería está muy rota, la nerfearán." },
    { id: 53, tema: 41, usuarioCreador: 3, texto: "Mi récord: 1.250.000 puntos." },
    { id: 54, tema: 41, usuarioCreador: 5, texto: "Eso es imposible, seguro que es trampa." },
    { id: 55, tema: 42, usuarioCreador: 4, texto: "Empieza siempre construyendo granjas." },
  ];

  // Denuncia: denunciante (1), denunciado (Usuario 0..1), contenidoDenunciado (Contenido 0..1)
  const denuncias = [
    { id: 60, fecha: daysAgo(10), estado: "Pendiente", denunciante: 5, denunciado: 3, contenidoDenunciado: 53 },
    { id: 61, fecha: daysAgo(9), estado: "Aprobado", denunciante: 3, denunciado: 5, contenidoDenunciado: 54 },
    { id: 62, fecha: daysAgo(5), estado: "Rechazado", denunciante: 3, denunciado: 4, contenidoDenunciado: 14 },
    { id: 63, fecha: daysAgo(1), estado: "Pendiente", denunciante: 6, denunciado: 4, contenidoDenunciado: null },
  ];

  window.WG = { usuarios, categorias, videojuegos, valoraciones, temas, comentarios, denuncias };
})();
