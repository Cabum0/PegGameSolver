import React, { useState } from 'react';
import config from './config';


const Controles = ({ onDatosRegresados }) => {
    const [numeroHoyos, setNumeroHoyos] = useState(0);
    const [cargando, setCargando] = useState(false);

    const manejarCambio = (event) => {
        setNumeroHoyos(event.target.value);
    };

    const manejarCalculo = async () => {
        setCargando(true);
        try {
            const response = await fetch(`${config.API_URL}?numeroHoyos=${numeroHoyos}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            if (!response.ok) {
                throw new Error('Error en la petición');
            }

            const data = await response.json();

            onDatosRegresados({
                
                totalClavijas: data.totalClavijas,
                totalMovimientos: data.totalMovimientos,
                mensajesStatus: data.mensajesStatus,
                movimientosOutput: data.MovimientosOutput,
                historicoEstadosTablero: data.HistoricoEstadosTablero
            });

        } catch (error) {
            console.error('Error:', error);
        } finally {
            setCargando(false); // Finalizar el estado de carga
        }
    };

    const manejarKeyDown = (event) => {
        if (event.key === 'Enter') {
            manejarCalculo(); // Ejecutar el cálculo al presionar Enter
        }
    };

    const borrarInput = () => {
        setNumeroHoyos('');
    };

    return (
        <div className="controles-container">
            {cargando && (
                <div className="loading-overlay">
                    <div className="loading-spinner"></div>
                    <p>Cargando...</p>
                </div>
            )}
            <table className="tabla_controles">
                <tbody>
                    <tr>
                        <td>Cantidad hoyos: </td>
                        <td>
                            <input
                                type="number"
                                id="numerohoyos"
                                name="numero"
                                min="0"
                                step="1"
                                value={numeroHoyos}
                                onChange={manejarCambio}
                                onClick={borrarInput}
                                onKeyDown={manejarKeyDown}
                            />
                        </td>
                        <td>
                            <button type="button" onClick={manejarCalculo} disabled={cargando}>Calcular</button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    );
};

export default Controles;
