﻿SELECT AVG(
                       ([hits] + [baseOnBalls] + [hitByPitch]) /
                       CAST(NULLIF([atBats] + [baseOnBalls] + [hitByPitch] + [sacrificeFlies], 0) AS [REAL]) +
                       (([hits] - [doubles] - [triples] - [homeruns]) + 2 * [doubles] + 3 * [triples] +
                        4 * [homeruns]) / CAST(NULLIF([atBats], 0) AS [REAL])
           )
           AS ops
FROM [v_baseball_player_info] vbpi
         LEFT JOIN t_baseball_player_local_ids tbpli ON vbpi.baseballPlayerGUID = tbpli.GUID
         LEFT JOIN t_stats_players tsp ON tbpli.localID = tsp.baseballPlayerLocalID
         LEFT JOIN t_stats ts ON tsp.statsPlayerID = ts.statsPlayerID
         LEFT JOIN t_stats_batting tsb ON ts.aggregatorID = tsb.aggregatorID
         LEFT JOIN t_baseball_players tbp ON tbpli.GUID = tbp.GUID
         LEFT JOIN t_season_stats tss ON ts.aggregatorID = tss.aggregatorID
         JOIN t_seasons tsea ON tss.seasonID = tsea.ID
WHERE tsea.ID = @seasonId