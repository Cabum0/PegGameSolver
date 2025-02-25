import { useState } from "react";
import "./App.css";
import MensajesEstatus from "./mensajes";
import ClavijasTable from "./visorClavijas";
import Controles from "./controles";
import Movimientos from "./movimientos";

function App() {
    const [datosRegresados, setDatosRegresados] = useState(null);
    const [tableroEstados, setTableroEstados] = useState([]);
    const [estadoActual, setEstadoActual] = useState(0);

    const manejarDatosRegresados = (datos) => {
        //console.log("Datos recibidos de la API:", datos);
        setDatosRegresados(datos);
        setTableroEstados(null);

        if (datos && datos.movimientosOutput?.length > 0) {
            const estados = datos.historicoEstadosTablero;
            setTableroEstados(estados);
            setEstadoActual(0);
        }
    };

    return (
        <>
            <h2 className="titulo_pagina">Peg Game Solver</h2>

            <Controles onDatosRegresados={manejarDatosRegresados} />
            {datosRegresados && <ClavijasTable data={tableroEstados ? tableroEstados[estadoActual] : null} />}
            {datosRegresados && (
                <Movimientos movimientos={datosRegresados.movimientosOutput} onMovimientoClick={setEstadoActual} />
            )}
            {datosRegresados && <MensajesEstatus strings={datosRegresados.mensajesStatus} />}
        </>
    );
}

export default App;