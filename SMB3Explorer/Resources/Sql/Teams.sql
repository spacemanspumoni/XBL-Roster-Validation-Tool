SELECT  t.GUID                  AS teamId,
        t.originalGUID          AS teamOriginalId,
        t.teamName              as teamName
FROM t_teams t;
-- TODO: Filter by league?