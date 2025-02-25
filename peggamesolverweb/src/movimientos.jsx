import React, { useState } from "react";

const Movimientos = ({ movimientos, onMovimientoClick }) => {
    const [selectedRow, setSelectedRow] = useState(null); // Estado para la fila seleccionada

    if (!movimientos || movimientos.length === 0) {
        return (
            <div className="no-movimientos-container">
                <p>No hay movimientos disponibles</p>
            </div>
        );
    }

    
    const movimientosConInicial = [
        { Origen: { fila: 0, columna: 0 }, Destino: { fila: 0, columna: 0 }, descripcion: "Posicion Inicial" },
        ...movimientos
    ];

    return (
        <div className="movimientos-container">
            <table className="tabla_movimientos" style={{ width: "100%", borderCollapse: "collapse" }}>
                <thead>
                    <tr>
                        <th className="border border-gray-400 p-2">#</th>
                        <th className="border border-gray-400 p-2">Movimientos (Da click para visualizar)</th>
                    </tr>
                </thead>
                <tbody>
                    {movimientosConInicial.map((mov, index) => (
                        <tr key={index}
                            className={`hover:bg-gray-100 transition-colors cursor-pointer ${selectedRow === index ? 'bg-blue-200' : ''}`}
                            onClick={() => {
                                setSelectedRow(index);
                                onMovimientoClick(index);
                            }}
                        >
                            <td className="border border-gray-400 p-2 text-center font-bold">{index}</td>
                            <td className="border border-gray-300 p-2">
                                {mov.descripcion === "Inicial"
                                    ? "Inicial"
                                    : `fila:${mov.Origen.fila}, columna:${mov.Origen.columna} --> fila:${mov.Destino.fila}, columna:${mov.Destino.columna}`}
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default Movimientos;
