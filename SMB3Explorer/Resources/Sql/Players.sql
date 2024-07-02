SELECT
    p.GUID as Id,
	firstName.optionValue as FirstName,
	lastName.optionValue as LastName,
	p.power as Power,
	p.contact as Contact,
	p.speed as Speed,
	p.fielding as Fielding,
	p.arm as Arm,
	p.velocity as Velocity,
	p.junk as Junk,
	p.accuracy as Accuracy,
	position.optionValue as Position,
	batting.optionValue as Batting,
	throwing.optionValue as Throwing
FROM t_baseball_players p
INNER JOIN t_baseball_player_local_ids lid
    ON lid.GUID = p.GUID
INNER JOIN t_baseball_player_options firstName
    ON firstName.baseballPlayerLocalID = lid.localID and firstName.optionKey = 66
INNER JOIN t_baseball_player_options lastName
    ON lastName.baseballPlayerLocalID = lid.localID and lastName.optionKey = 67
INNER JOIN t_baseball_player_options position
	ON position.baseballPlayerLocalID = lid.localID and position.optionKey = 57
INNER JOIN t_baseball_player_options batting
	ON batting.baseballPlayerLocalID = lid.localID and batting.optionKey = 5
INNER JOIN t_baseball_player_options throwing
	ON throwing.baseballPlayerLocalID = lid.localID and throwing.optionKey = 4
LEFT JOIN t_baseball_player_traits t
    ON t.baseballPlayerLocalID = lid.localID
WHERE p.teamGUID = CAST(@teamID AS BLOB)
GROUP BY p.GUID