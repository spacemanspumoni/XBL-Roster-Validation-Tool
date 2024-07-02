SELECT
    p.GUID as Id,
	vbpi.firstName as FirstName,
	vbpi.lastName as LastName,
	p.power as Power,
	p.contact as Contact,
	p.speed as Speed,
	p.fielding as Fielding,
	p.arm as Arm,
	p.velocity as Velocity,
	p.junk as Junk,
	p.accuracy as Accuracy,
	vbpi.primaryPosition as PrimaryPosition,
	secondaryPosition.optionValue as SecondaryPosition,
	vbpi.pitcherRole as PitchPosition,
	batting.optionValue as Batting,
	throwing.optionValue as Throwing,
	chemistry.optionValue as Chemistry,
	fourSeam.optionValue as FourSeam,
	twoSeam.optionValue as TwoSeam,
	SB.optionValue as Screwball,
	CH.optionValue as ChangeUp,
	FK.optionValue as Fork,
	CB.optionValue as Curve,
	SL.optionValue as Slider,
	CF.optionValue as Cutter,
	armAngle.optionValue as ArmAngle,
	CASE
        WHEN COUNT(traits.trait) = 0 THEN NULL
        ELSE json_group_array(json_object('traitId', traits.trait, 'subtypeId', traits.subType))
    END AS Traits
FROM t_baseball_players p
INNER JOIN t_baseball_player_local_ids lid
    ON lid.GUID = p.GUID
INNER JOIN [v_baseball_player_info] vbpi
	ON vbpi.baseballPlayerGUID = lid.GUID
INNER JOIN t_baseball_player_options batting
	ON batting.baseballPlayerLocalID = lid.localID and batting.optionKey = 5
INNER JOIN t_baseball_player_options throwing
	ON throwing.baseballPlayerLocalID = lid.localID and throwing.optionKey = 4
INNER JOIN t_baseball_player_options chemistry
	ON chemistry.baseballPlayerLocalID = lid.localID and chemistry.optionKey = 107
LEFT JOIN t_baseball_player_options secondaryPosition
    ON secondaryPosition.baseballPlayerLocalID = lid.localID AND secondaryPosition.optionKey = 55
LEFT JOIN t_baseball_player_options fourSeam
	ON fourSeam.baseballPlayerLocalID = lid.localID and fourSeam.optionKey = 58
LEFT JOIN t_baseball_player_options twoSeam
	ON twoSeam.baseballPlayerLocalID = lid.localID and twoSeam.optionKey = 59
LEFT JOIN t_baseball_player_options SB
	ON SB.baseballPlayerLocalID = lid.localID and SB.optionKey = 60
LEFT JOIN t_baseball_player_options CH
	ON CH.baseballPlayerLocalID = lid.localID and CH.optionKey = 61
LEFT JOIN t_baseball_player_options FK
	ON FK.baseballPlayerLocalID = lid.localID and FK.optionKey = 62
LEFT JOIN t_baseball_player_options CB
	ON CB.baseballPlayerLocalID = lid.localID and CB.optionKey = 63
LEFT JOIN t_baseball_player_options SL
	ON SL.baseballPlayerLocalID = lid.localID and SL.optionKey = 64
LEFT JOIN t_baseball_player_options CF
	ON CF.baseballPlayerLocalID = lid.localID and CF.optionKey = 65
LEFT JOIN t_baseball_player_options armAngle
	ON armAngle.baseballPlayerLocalID = lid.localID and armAngle.optionKey = 49
LEFT JOIN t_baseball_player_traits traits
    ON traits.baseballPlayerLocalID = lid.localID
WHERE p.teamGUID = CAST(@teamID AS BLOB)
GROUP BY p.GUID, lid.localID