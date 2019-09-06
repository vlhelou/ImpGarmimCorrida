use Corrida
go
declare @corte int = 75
select 
	Corrida.Id,
	Corrida.Inicio,
	sum(case when Track.CadenciaPasso>=@corte then 1 else 0 end) Corrida,
	sum(case when Track.CadenciaPasso<@corte then 1 else 0 end) Caminha,
	count(1) Total,
	convert(money, sum(case when Track.CadenciaPasso>=@corte then 1 else 0 end) )/count(1)*100 as PctCorrida
from Corrida
	inner join Track on Corrida.Id = Track.IdCorrida
group by
	Corrida.Id,
	Corrida.Inicio
order by Inicio desc