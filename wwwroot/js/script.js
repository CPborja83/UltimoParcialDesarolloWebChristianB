// Función para listar los cines en la tabla
async function listarCines() {
    try {
        const response = await fetch('https://localhost:7295/api/cine/listarCines'); // URL de la API para listar cines
        if (!response.ok) {
            throw new Error('Error al obtener la lista de cines');
        }
        const cines = await response.json(); // Convierte la respuesta a JSON

        const tablaCines = document.getElementById('cineTableBody'); // Cuerpo de la tabla en el HTML
        tablaCines.innerHTML = ''; // Limpiar el contenido anterior de la tabla

        // Iterar sobre cada cine y agregar una fila a la tabla
        cines.forEach(cine => {
            const row = document.createElement('tr'); // Crear una nueva fila
            const nombreCell = document.createElement('td');
            nombreCell.textContent = cine.nombre; // Mostrar el nombre del cine

            const ubicacionCell = document.createElement('td');
            ubicacionCell.textContent = cine.ubicacion; // Mostrar la ubicación del cine

            const accionesCell = document.createElement('td');
            const eliminarButton = document.createElement('button');
            eliminarButton.textContent = 'Eliminar';
            eliminarButton.className = 'btn btn-danger';
            eliminarButton.onclick = function () {
                eliminarCine(cine.cineID); // Llamar a la función eliminarCine
            };
            accionesCell.appendChild(eliminarButton); // Añadir botón de eliminar a la celda de acciones

            row.appendChild(nombreCell);
            row.appendChild(ubicacionCell);
            row.appendChild(accionesCell);

            tablaCines.appendChild(row); // Añadir la fila a la tabla
        });
    } catch (error) {
        console.error('Error al listar cines:', error);
    }
}

// Función para cargar cines en el select (dropdown)
function cargarCines() {
    fetch('https://localhost:7295/api/cine/listarCines')
        .then(response => response.json())
        .then(data => {
            if (Array.isArray(data)) {
                const cineSelect = document.getElementById('cineSelect'); // Select para los cines
                cineSelect.innerHTML = '<option value="">Selecciona un cine</option>';
                data.forEach(cine => {
                    cineSelect.innerHTML += `<option value="${cine.cineID}">${cine.nombre}</option>`;
                });
            } else {
                console.error('La respuesta de cines no es un arreglo.');
            }
        })
        .catch(error => console.error('Error al cargar cines:', error));
}

// Función para eliminar un cine
function eliminarCine(id) {
    fetch(`https://localhost:7295/api/cine/eliminar/${id}`, { // URL con el ID del cine a eliminar
        method: 'DELETE',
    })
        .then(response => {
            if (response.ok) {
                listarCines(); // Volver a listar cines después de eliminar
            } else {
                console.error('Error al eliminar cine:', response.statusText);
            }
        })
        .catch(error => console.error('Error al eliminar cine:', error));
}

function eliminarPelicula(id) {
    fetch(`https://localhost:7295/api/cine/eliminarPelicula/${id}`, { // URL con el ID del cine a eliminar
        method: 'DELETE',
    })
        .then(response => {
            if (response.ok) {
                listarPeliculas(); // Volver a listar cines después de eliminar
            } else {
                console.error('Error al eliminar pelicula:', response.statusText);
            }
        })
        .catch(error => console.error('Error al eliminar pelicula:', error));
}

// Función para listar películas
function listarPeliculas() {
    fetch('https://localhost:7295/api/cine/listarPeliculas')
        .then(response => {
            if (!response.ok) {
                throw new Error('Error al listar películas: ' + response.statusText);
            }
            return response.json();
        })
        .then(data => {
            console.log('Datos recibidos:', data); // Depuración: Verifica los datos recibidos

            $('#peliculaTableBody').empty(); // Limpiar la tabla
            data.forEach(pelicula => {
                const fechaEstreno = pelicula.fechaEstreno
                    ? new Date(pelicula.fechaEstreno).toLocaleDateString('es-GT')
                    : 'Fecha no disponible'; // Manejar si la fecha es nula

                $('#peliculaTableBody').append(`
                    <tr>
                        <td>${pelicula.cineNombre}</td>
                        <td>${pelicula.titulo}</td>
                        <td>${pelicula.generoNombre}</td>
                        <td>${pelicula.sinopsis || 'Sin sinopsis disponible'}</td> <!-- Manejo de sinopsis nula -->
                        <td>${pelicula.fechaEstreno}</td> <!-- Formateo de fecha -->
                        <td><button class="btn btn-danger" onclick="eliminarPelicula(${pelicula.peliculaID})">Eliminar</button></td>
                    </tr>
                `);
            });
        })
        .catch(error => console.error('Error al listar películas:', error));
}


// Función para agregar un nuevo cine
document.getElementById('formAgregarCine').addEventListener('submit', function (event) {
    event.preventDefault(); // Prevenir el comportamiento por defecto del formulario
    const cineNombre = document.getElementById('nombreCine').value; // Obtener el nombre del cine
    const cineUbicacion = document.getElementById('ubicacionCine').value; // Obtener la ubicación del cine

    fetch('https://localhost:7295/api/cine/guardar', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ nombre: cineNombre, ubicacion: cineUbicacion }), // Datos a enviar
    })
        .then(response => {
            if (response.ok) {
                listarCines();
                cargarCines(); // Actualiza el select de cines
                document.getElementById('formAgregarCine').reset(); // Limpiar el formulario
            } else {
                console.error('Error al agregar cine:', response.statusText);
            }
        })
        .catch(error => console.error('Error al agregar cine:', error));
});

// Función para agregar una nueva película
document.getElementById('peliculaForm').addEventListener('submit', function (event) {
    event.preventDefault(); // Prevenir el comportamiento por defecto del formulario
    const cineSelect = document.getElementById('cineSelect').value; // Obtener el cine seleccionado
    const peliculaTitulo = document.getElementById('peliculaTitulo').value; // Obtener el título de la película
    const peliculaSinopsis = document.getElementById('peliculaSinopsis').value; // Obtener la sinopsis de la película
    const peliculaFechaEstreno = new Date(document.getElementById('peliculaFechaEstreno').value).toISOString(); // Obtener la fecha de estreno
    const peliculaGenero = document.getElementById('peliculaGenero').value; // Obtener el género

    // Mapeo del género a su ID correspondiente en la base de datos
    let generoID;
    switch (peliculaGenero) {
        case 'Terror':
            generoID = 1; // Asigna el ID correcto para Terror
            break;
        case 'Suspenso':
            generoID = 2; // Asigna el ID correcto para Suspenso
            break;
        case 'Comedia':
            generoID = 3; // Asigna el ID correcto para Comedia
            break;
        case 'Acción':
            generoID = 4; // Asigna el ID correcto para Acción
            break;
        default:
            console.error('Género no válido');
            return; // Salir si el género no es válido
    }

    // Realizar la solicitud POST
    fetch('https://localhost:7295/api/cine/insertarPelicula', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            CineID: parseInt(cineSelect), // Asegúrate de que esto coincide con la propiedad de tu modelo y es un entero
            Titulo: peliculaTitulo, // Asegúrate de que esto coincide
            Sinopsis: peliculaSinopsis, // Asegúrate de que esto coincide
            FechaEstreno: peliculaFechaEstreno, // Asegúrate de que esto coincide
            GeneroID: generoID // Asegúrate de que esto coincide
        }),
    })
        .then(response => {
            if (response.ok) {
                listarPeliculas();
                document.getElementById('peliculaForm').reset(); // Limpiar el formulario
            } else {
                response.text().then(errorMessage => {
                    console.error('Error al agregar película:', response.statusText, errorMessage);
                });
            }
        })
        .catch(error => console.error('Error al agregar película:', error));
});


// Ejecutar las funciones para listar cines y cargar los selectores al cargar la página
document.addEventListener('DOMContentLoaded', function () {
    listarCines(); // Cargar la lista de cines
    cargarCines(); // Cargar cines en el selector
    listarPeliculas(); // Cargar la lista de películas
});
