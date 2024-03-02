ID: FALLEN_TREE
IMAGE: FALLEN_TREE_IMG
PARAMS:
    mossberga 590a1 Character1_Name:        @Strongest.Name
    Character1_Strenght:    @Strongest.Strenght
    Distance:               @Distance
TEXT:
    asfasghakl jshfs akj lhfasl kjhfaslkjfsa
    asfhjk lashf ljkashflk sajhfljk sahflkjash
    sa hjfkla lkjs ahflk ashlkj fash jfsk alhkjas
    saklasd {Character1_Name} fdsafas dhgdash dsah
EFFECTS:
    Character1_Strenght + 1
OPTIONS:
    LIFT_TREE "Character1_Name Lifts tree" when Character1_Strenght >= 4
    DETOUR "take detour"
    FOUND_HIDDEN_PATH "@Perceptiest.Name found something..."
    SHOOT_TREE "Everybody shoots tree" when
        @total_ammo > 10 :w

        @has_guns
        @atleast.one.can_use_guns