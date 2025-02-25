export default function MensajesEstatus({ strings }) {
    if (!strings || strings.length === 0) return <p className="mensaje-vacio">No hay datos disponibles</p>;

    const mensajesValidos = strings.filter(item => item); // Filtrar mensajes no nulos

    if (mensajesValidos.length === 0) return <p className="mensaje-vacio">No hay mensajes disponibles</p>;

    return (
        <div className="mensajes_box centrar_box">
            <h2 className="titulo-ejecucion">Log ejecucion</h2>
            <table className="tabla-mensajes">
                <thead>
                    <tr className="cabecera">
                        <th className="border p-2 text-left">#</th>
                        <th className="border p-2 text-left">Mensaje</th>
                    </tr>
                </thead>
                <tbody>
                    {mensajesValidos.map((item, index) => (
                        <tr key={index} className="fila-mensaje">
                            <td className="border p-2">{index + 1}</td>
                            <td className="border p-2">{item}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}
