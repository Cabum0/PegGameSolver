const ClavijasTable = ({ data }) => {
    // Verificar si el arreglo es nulo o está vacío
    if (!data || data.length === 0) {
        return <p>No hay datos disponibles</p>;
    }

    return (
        <table className="visor_clavijas border-collapse border border-gray-400">
            <tbody>
                {/* Encabezado de columnas */}
                <tr>
                    <th className="border border-gray-400 p-2">#</th>
                    {data[0].map((_, cellIndex) => (
                        <th key={cellIndex} className="coordenada border border-gray-400 p-2">{cellIndex}</th>
                    ))}
                </tr>

                {data.map((row, rowIndex) => (
                    <tr key={rowIndex}>
                        {/* Número de fila */}
                        <td className="coordenada border border-gray-400 p-2 font-bold">{rowIndex}</td>
                        {row.map((cell, cellIndex) => (
                            <td key={cellIndex} className="border border-gray-300 p-2">
                                <div
                                    className={`base_clavija_hueco ${cell === 'x' ? 'clavija' : 'hueco'}`}
                                ></div>
                            </td>
                        ))}
                    </tr>
                ))}
            </tbody>
        </table>
    );
};

export default ClavijasTable;
