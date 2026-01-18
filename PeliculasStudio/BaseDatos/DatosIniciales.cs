using PeliculasStudio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace PeliculasStudio.BaseDatos
{
    public static class DatosIniciales
    {

        public static List<Pelicula> ObtenerListado()
        {
            return new List<Pelicula>
            {
                 new Pelicula {
                        Titulo = "Blade Runner 2049",
                        Anio = 2017,
                        Genero = "Ciencia Ficcion",
                      Resumen = @"En el año 2049, un nuevo blade runner...
                                    descubre un secreto oculto que podría
                                    cambiar el mundo.",
                        TrailerPath = "Blade Runner 2049.mp4",
                        PortadaPath = "Blade_Runner.jpg"
                    },
                    new Pelicula {
                        Titulo = "La Odisea",
                        Anio = 2026,
                        Genero = "Aventura",
                        Resumen = @"Una epopeya mitológica que sigue la historia de Odiseo
                                    y su largo viaje a casa, de 10 años de duración, tras la guerra de Troya.",
                        TrailerPath = "La Odisea.mp4",
                        PortadaPath = "Odisea.jpg"
                    },
                    new Pelicula {
                Titulo = "Expediente Warren: El último rito",
                Anio = 2021,
                Genero = "Terror",
                Resumen = @"Los investigadores de lo paranormal Ed y Lorraine Warren
                                se enfrentan a un último caso aterrador
                        en el que están implicadas entidades misteriosas a las que deben enfrentarse.",
                TrailerPath = "Expediente-Warren-El-ultimo-rito.mp4",
                PortadaPath = "Expediente_Warrem_El_Ultimo_Rito.jpg"
            },
                    new Pelicula {
                Titulo = "El Diario de Noa",
                Anio = 2004,
                Genero = "Romance",
                Resumen = @"En una residencia de ancianos, un hombre (James Garner) lee a una mujer (Gena Rowlands)
                            una historia de amor escrita en su viejo cuaderno de notas.
                        Es la historia de Noah Calhoun (Ryan Gosling) y Allie Hamilton (Rachel McAdams)
                    , dos jóvenes adolescentes de Carolina del Norte que, 
                    a pesar de vivir en dos ambientes sociales muy diferentes,
                    se enamoraron profundamente y pasaron juntos un verano inolvidable, antes de ser separados,
                    primero por sus padres, y más tarde por la guerra.",
                TrailerPath = "El Diario de Noa.mp4",
                PortadaPath = "EL_Diario_De_Noa.jpg"
            },

                    new Pelicula {
                    Titulo = "21 Black Jack",
                    Anio = 2008,
                    Genero = "Drama",
                    Resumen = "Ben Campbell, un tímido y brillante estudiante del M.I.T., recurre a los naipes y a un grupo de estudiantes liderados por un profesor para ganar una fortuna en Las Vegas.",
                    TrailerPath = "21 Black Jack.mp4",
                    PortadaPath = "21 Black Jack.jpg"
                },
                new Pelicula {
                    Titulo = "American History X",
                    Anio = 1998,
                    Genero = "Drama",
                    Resumen = "Derek Vinyard, un joven líder neonazi, va a prisión por asesinato. Al salir, intenta evitar que su hermano pequeño siga el mismo camino de odio y violencia.",
                    TrailerPath = "American History X.mp4",
                    PortadaPath = "American History X.jpg"
                },
                new Pelicula {
                    Titulo = "Vengadores: Endgame",
                    Anio = 2019,
                    Genero = "Acción",
                    Resumen = "Tras los devastadores eventos de Infinity War, el universo está en ruinas. Con la ayuda de los aliados restantes, los Vengadores se reúnen para deshacer las acciones de Thanos.",
                    TrailerPath = "Avengers_EndGame.mp4",
                    PortadaPath = "Avengers_EndGame.jpg"
                },
                new Pelicula {
                    Titulo = "Braveheart",
                    Anio = 1995,
                    Genero = "Acción",
                    Resumen = "En el siglo XIV, los escoceses viven oprimidos por los ingleses. William Wallace regresa a su tierra y lidera un levantamiento contra el rey Eduardo I.",
                    TrailerPath = "Braveheart.mp4",
                    PortadaPath = "Braveheart.jpg"
                },
                new Pelicula {
                    Titulo = "Django Desencadenado",
                    Anio = 2012,
                    Genero = "Western",
                    Resumen = "Acompañado por un cazarrecompensas alemán, un esclavo liberado viaja a través de Estados Unidos para rescatar a su esposa de un sádico propietario de plantaciones.",
                    TrailerPath = "Django Desencadenado.mp4",
                    PortadaPath = "Django Desencadenado.jpg"
                },
                new Pelicula {
                    Titulo = "Dune",
                    Anio = 2021,
                    Genero = "Ciencia Ficción",
                    Resumen = "Paul Atreides, un joven brillante nacido con un destino más grande que él mismo, debe viajar al planeta más peligroso del universo para asegurar el futuro de su familia y su pueblo.",
                    TrailerPath = "Dune.mp4",
                    PortadaPath = "Dune.jpg"
                },
                new Pelicula {
                    Titulo = "Dune: Parte Dos",
                    Anio = 2024,
                    Genero = "Ciencia Ficción",
                    Resumen = "Paul Atreides se une a Chani y a los Fremen mientras busca venganza contra los conspiradores que destruyeron a su familia.",
                    TrailerPath = "Dune Parte Dos.mp4",
                    PortadaPath = "Dune Parte Dos.jpg"
                },
                new Pelicula {
                    Titulo = "El Club de la Lucha",
                    Anio = 1999,
                    Genero = "Drama",
                    Resumen = "Un oficinista insomne y un desentendido fabricante de jabón forman un club de lucha clandestino que se convierte en algo mucho más grande.",
                    TrailerPath = "El Club de la Lucha.mp4",
                    PortadaPath = "El Club de la Lucha.jpg"
                },
                new Pelicula {
                    Titulo = "El Lobo de Wall Street",
                    Anio = 2013,
                    Genero = "Comedia",
                    Resumen = "Basada en la historia real de Jordan Belfort, desde su ascenso a un estilo de vida decadente como corredor de bolsa hasta su caída involucrando crimen y corrupción.",
                    TrailerPath = "El lobo de Wall Street.mp4",
                    PortadaPath = "El lobo de Wall Street.jpg"
                },
                new Pelicula {
                    Titulo = "El Resplandor",
                    Anio = 1980,
                    Genero = "Terror",
                    Resumen = "Jack Torrance se traslada con su mujer y su hijo al hotel Overlook para encargarse del mantenimiento. Poco a poco, comienza a sufrir inquietantes trastornos de personalidad.",
                    TrailerPath = "EL RESPLANDOR.mp4",
                    PortadaPath = "EL RESPLANDOR.jpg"
                },
                new Pelicula {
                    Titulo = "El Rey León",
                    Anio = 2019,
                    Genero = "Aventura",
                    Resumen = "Tras el asesinato de su padre, Simba, un joven león, huye de su reino solo para aprender el verdadero significado de la responsabilidad y la valentía.",
                    TrailerPath = "El Rey León 2019.mp4",
                    PortadaPath = "El Rey León 2019.jpg"
                },
                new Pelicula {
                    Titulo = "El Silencio de los Corderos",
                    Anio = 1991,
                    Genero = "Thriller",
                    Resumen = "Una joven cadete del FBI debe buscar la ayuda de un asesino caníbal encarcelado y manipulador para ayudar a atrapar a otro asesino en serie.",
                    TrailerPath = "El silencio de los corderos.mp4",
                    PortadaPath = "El silencio de los corderos.jpg"
                },
                new Pelicula {
                    Titulo = "El Caballero Oscuro",
                    Anio = 2008,
                    Genero = "Acción",
                    Resumen = "Batman se enfrenta a su mayor amenaza hasta el momento: el Joker, una mente criminal que sumerge a Gotham en la anarquía.",
                    TrailerPath = "El-Caballero-Oscuro.mp4",
                    PortadaPath = "El-Caballero-Oscuro.jpg"
                },
                new Pelicula {
                    Titulo = "Forrest Gump",
                    Anio = 1994,
                    Genero = "Drama",
                    Resumen = "Las presidencias de Kennedy y Johnson, la guerra de Vietnam y otros eventos históricos se desarrollan a través de la perspectiva de un hombre de Alabama con un CI bajo.",
                    TrailerPath = "Forrest Gump.mp4",
                    PortadaPath = "Forrest Gump.jpg"
                },
                new Pelicula {
                    Titulo = "Furiosa: A Mad Max Saga",
                    Anio = 2024,
                    Genero = "Acción",
                    Resumen = "La historia de origen de la guerrera renegada Furiosa antes de su encuentro y unión con Mad Max.",
                    TrailerPath = "Furiosa de la saga Mad Max.mp4",
                    PortadaPath = "Furiosa de la saga Mad Max.jpg"
                },
                new Pelicula {
                    Titulo = "Gladiator",
                    Anio = 2000,
                    Genero = "Acción",
                    Resumen = "Un antiguo general romano se propone vengarse del emperador corrupto que asesinó a su familia y lo envió a la esclavitud.",
                    TrailerPath = "Gladiator.mp4",
                    PortadaPath = "Gladiator.jpg"
                },
                new Pelicula {
                    Titulo = "Gladiator II",
                    Anio = 2024,
                    Genero = "Acción",
                    Resumen = "Años después de presenciar la muerte del venerado héroe Máximo, Lucio se ve obligado a entrar en el Coliseo.",
                    TrailerPath = "Gladiator2.mp4",
                    PortadaPath = "Gladiator2.jpg"
                },
                new Pelicula {
                    Titulo = "Guardianes de la Galaxia Vol. 3",
                    Anio = 2023,
                    Genero = "Ciencia Ficción",
                    Resumen = "Peter Quill, aún conmocionado por la pérdida de Gamora, debe reunir a su equipo para defender el universo y proteger a uno de los suyos.",
                    TrailerPath = "Guardianes de la Galaxia Volumen 3.mp4",
                    PortadaPath = "Guardianes de la Galaxia Volumen 3.jpg"
                },
                new Pelicula {
                    Titulo = "In Time",
                    Anio = 2011,
                    Genero = "Ciencia Ficción",
                    Resumen = "En un futuro donde la gente deja de envejecer a los 25 años y el tiempo es la moneda, un hombre es acusado falsamente de asesinato y debe huir.",
                    TrailerPath = "In Time.mp4",
                    PortadaPath = "In Time.jpg"
                },
                new Pelicula {
                    Titulo = "Joker",
                    Anio = 2019,
                    Genero = "Drama",
                    Resumen = "En Gotham City, el comediante con problemas mentales Arthur Fleck es ignorado y maltratado por la sociedad, lo que lo lleva a una espiral de crimen.",
                    TrailerPath = "JOKER.mp4",
                    PortadaPath = "JOKER.jpg"
                },
                new Pelicula {
                    Titulo = "La Monja",
                    Anio = 2018,
                    Genero = "Terror",
                    Resumen = "Un sacerdote con un pasado embrujado y una novicia a punto de hacer sus votos finales son enviados por el Vaticano para investigar la muerte de una joven monja.",
                    TrailerPath = "La Monja.mp4",
                    PortadaPath = "La Monja.jpg"
                },
                new Pelicula {
                    Titulo = "La Monja II",
                    Anio = 2023,
                    Genero = "Terror",
                    Resumen = "1956 - Francia. Un sacerdote es asesinado. Un mal se está extendiendo. La hermana Irene se enfrenta una vez más a Valak, la monja demonio.",
                    TrailerPath = "La monja II.mp4",
                    PortadaPath = "La monja II.jpg"
                },
               
                new Pelicula {
                    Titulo = "La vida es bella",
                    Anio = 1997,
                    Genero = "Drama",
                    Resumen = "Un judío italiano dueño de una librería emplea su fértil imaginación para proteger a su hijo de los horrores de un campo de concentración nazi.",
                    TrailerPath = "La vida es bella.mp4",
                    PortadaPath = "La vida es bella.jpg"
                },
                new Pelicula {
                    Titulo = "Memento",
                    Anio = 2000,
                    Genero = "Thriller",
                    Resumen = "Un hombre crea un extraño sistema para ayudarse a recordar cosas; para así poder cazar al asesino de su esposa sin que su pérdida de memoria a corto plazo se interponga.",
                    TrailerPath = "Memento.mp4",
                    PortadaPath = "Memento.jpg"
                },
                new Pelicula {
                    Titulo = "Oppenheimer",
                    Anio = 2023,
                    Genero = "Drama",
                    Resumen = "La historia del científico estadounidense J. Robert Oppenheimer y su papel en el desarrollo de la bomba atómica.",
                    TrailerPath = "OPPENHEIMER.mp4",
                    PortadaPath = "OPPENHEIMER.jpg"
                },
                new Pelicula {
                    Titulo = "Orgullo y Prejuicio",
                    Anio = 2005,
                    Genero = "Romance",
                    Resumen = "Las chispas vuelan cuando la enérgica Elizabeth Bennet conoce al soltero, rico y orgulloso Sr. Darcy. Pero el Sr. Darcy se enamora a regañadientes.",
                    TrailerPath = "Orgullo y prejuicio.mp4",
                    PortadaPath = "Orgullo y prejuicio.jpg"
                },
                new Pelicula {
                    Titulo = "Origen Inception",
                    Anio = 2010,
                    Genero = "Ciencia Ficción",
                    Resumen = "A un ladrón que roba secretos corporativos a través del uso de la tecnología de compartir sueños se le da la tarea inversa de plantar una idea en la mente de un director ejecutivo.",
                    TrailerPath = "Origen Inception.mp4",
                    PortadaPath = "Origen Inception.jpg"
                },
                new Pelicula {
                    Titulo = "Pulp Fiction",
                    Anio = 1994,
                    Genero = "Crimen",
                    Resumen = "Las vidas de dos sicarios de la mafia, un boxeador, la esposa de un gángster y un par de bandidos se entrelazan en cuatro historias de violencia y redención.",
                    TrailerPath = "Pulp Fiction.mp4",
                    PortadaPath = "Pulp Fiction.jpg"
                },

                new Pelicula {
                    Titulo = "Réquiem por un sueño",
                    Anio = 2000,
                    Genero = "Drama",
                    Resumen = "Las utopías inducidas por las drogas de cuatro personas de Coney Island se hacen añicos cuando sus adicciones se vuelven más profundas.",
                    TrailerPath = "Requiem por un sueño.mp4",
                    PortadaPath = "Requiem por un sueño.jpg"
                },
                new Pelicula {
                    Titulo = "Saw X",
                    Anio = 2023,
                    Genero = "Terror",
                    Resumen = "Un enfermo y desesperado John Kramer viaja a México para un procedimiento médico arriesgado y experimental, solo para descubrir que toda la operación es una estafa.",
                    TrailerPath = "Saw X.mp4",
                    PortadaPath = "Saw X.jpg"
                },
                new Pelicula {
                    Titulo = "Sinister",
                    Anio = 2012,
                    Genero = "Terror",
                    Resumen = "Un escritor de crímenes reales descubre un alijo de cintas de 8mm que muestran brutales asesinatos ocurridos en la casa que acaba de comprar.",
                    TrailerPath = "Sinister.mp4",
                    PortadaPath = "Sinister.jpg"
                },
                new Pelicula {
                    Titulo = "Sinister 2",
                    Anio = 2015,
                    Genero = "Terror",
                    Resumen = "Una joven madre y sus hijos gemelos se mudan a una casa rural marcada por la muerte. El espíritu maligno de Bughuul continúa propagándose con aterradora intensidad.",
                    TrailerPath = "Sinister 2.mp4",
                    PortadaPath = "Sinister 2.jpg"
                },
                new Pelicula {
                    Titulo = "Spider-Man: Cruzando el Multiverso",
                    Anio = 2023,
                    Genero = "Animación",
                    Resumen = "Miles Morales se catapulta a través del Multiverso, donde se encuentra con un equipo de Spider-Gente encargado de proteger su existencia.",
                    TrailerPath = "SPIDER-MAN CRUZANDO EL MULTIVERSO.mp4",
                    PortadaPath = "SPIDER-MAN CRUZANDO EL MULTIVERSO.jpg"
                },
                new Pelicula {
                    Titulo = "Taxi Driver",
                    Anio = 1976,
                    Genero = "Drama",
                    Resumen = "Un veterano de la guerra de Vietnam mentalmente inestable trabaja como taxista nocturno en Nueva York, donde la decadencia y sordidez alimentan su necesidad de violencia.",
                    TrailerPath = "TAXI DRIVER 1976.mp4",
                    PortadaPath = "TAXI DRIVER 1976.jpg"
                },
                new Pelicula {
                    Titulo = "Titanic",
                    Anio = 1997,
                    Genero = "Romance",
                    Resumen = "Una aristócrata de diecisiete años se enamora de un artista pobre pero amable a bordo del lujoso y malogrado R.M.S. Titanic.",
                    TrailerPath = "Titanic.mp4",
                    PortadaPath = "Titanic.jpg"
                },
                new Pelicula {
                    Titulo = "El Club de los Poetas Muertos",
                    Anio = 1989,
                    Genero = "Drama",
                    Resumen = "Un profesor de inglés inspira a sus estudiantes a mirar la poesía con una perspectiva diferente de conocimiento y sentimientos auténticos.",
                    TrailerPath = "Trailer El Club de los poetas Muertos.mp4",
                    PortadaPath = "Trailer El Club de los poetas Muertos.jpg"
                },
                new Pelicula {
                    Titulo = "El Señor de los Anillos: El Retorno del Rey",
                    Anio = 2003,
                    Genero = "Fantasía",
                    Resumen = "Gandalf y Aragorn lideran el Mundo de los Hombres contra el ejército de Sauron para distraer su atención de Frodo y Sam, quienes se acercan al Monte del Destino.",
                    TrailerPath = "Tráiler El Señor de los Anillos El Retorno del Rey.mp4",
                    PortadaPath = "Tráiler El Señor de los Anillos El Retorno del Rey.jpg"
                },
                new Pelicula {
                    Titulo = "Trainspotting",
                    Anio = 1996,
                    Genero = "Drama",
                    Resumen = "Renton, profundamente inmerso en la escena de las drogas de Edimburgo, intenta limpiarse y salir, a pesar del atractivo de las drogas y la influencia de sus amigos.",
                    TrailerPath = "Trainspotting.mp4",
                    PortadaPath = "Trainspotting.jpg"
                },
                new Pelicula {
                    Titulo = "Venganza",
                    Anio = 2008,
                    Genero = "Acción",
                    Resumen = "Un agente retirado de la CIA viaja por Europa y confía en sus viejas habilidades para salvar a su hija distanciada, que ha sido secuestrada durante un viaje a París.",
                    TrailerPath = "Venganza.mp4",
                    PortadaPath = "Venganza.jpg"
                }

        };
        }
    }
}
