SELECT i.IdInmueble, i.Direccion, i.Uso, i.Tipo, i.Ambientes, i.Precio, i.Estado, i.IdPropietario, p.Nombre, p.Apellido
FROM Inmueble i INNER JOIN Propietario p ON i.IdPropietario = p.IdPropietario;