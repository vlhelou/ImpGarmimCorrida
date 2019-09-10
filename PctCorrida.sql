use Corrida
go

declare @corte int = 75;

with difDistancia (IdCorrida, Id, Hora, Elevacao, Batimento, CadenciaPasso, lat, lon, Distancia, Percorrido) as (
	select 
		Track.IdCorrida
		,Track.Id
		, Track.Hora
		, Track.Elevacao
		, Track.Batimento
		, Track.CadenciaPasso
		, Track.lat
		, Track.lon
		, Track.Distancia
		, Proximo.Distancia - Track.Distancia as Percorrido
	from Track
		join Track as Proximo on 
			Track.IdCorrida = Proximo.IdCorrida and Proximo.Id = Track.Id+1
)


select 
	Corrida.Id,
	Corrida.Inicio,
	format(sum(case when difDistancia.CadenciaPasso>=@corte then Percorrido else 0 end),'N2') Corrida,
	format(sum(case when difDistancia.CadenciaPasso<@corte then Percorrido else 0 end),'N2') Caminha,
	count(1) TotalRegistros,
	format(sum(Percorrido),'N2') TotalPercorrido,
	convert(money, sum(case when difDistancia.CadenciaPasso>=@corte then Percorrido else 0 end) )/sum(Percorrido)*100 as PctCorrida
from Corrida
	join difDistancia on Corrida.Id = difDistancia.IdCorrida
group by
	Corrida.Id,
	Corrida.Inicio
order by Inicio desc
