-- Consultas usadas no vídeo para provar a persistência após cada operação

SELECT * FROM dbo.cliente ORDER BY id;

SELECT * FROM dbo.conta ORDER BY id;

-- relacionamento: contas com o nome do cliente dono
SELECT c.id AS conta_id, c.numero, c.tipo, c.saldo, cl.id AS cliente_id, cl.nome
FROM dbo.conta c
JOIN dbo.cliente cl ON cl.id = c.cliente_id
ORDER BY c.id;
