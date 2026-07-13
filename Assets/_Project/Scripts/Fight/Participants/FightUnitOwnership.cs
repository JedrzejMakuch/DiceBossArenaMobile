using System;
using UnityEngine;

[Serializable]
public sealed class FightUnitOwnership
{
    [SerializeField] private FightTeamId teamId;

    [SerializeField]
    private FightParticipantId participantId;

    [SerializeField]
    private FightControllerType controllerType;

    public FightTeamId TeamId => teamId;

    public FightParticipantId ParticipantId =>
        participantId;

    public FightControllerType ControllerType =>
        controllerType;

    public FightUnitOwnership(
        FightTeamId teamId,
        FightParticipantId participantId,
        FightControllerType controllerType)
    {
        this.teamId = teamId;
        this.participantId = participantId;
        this.controllerType = controllerType;
    }

    public FightUnitOwnership CreateForSummon()
    {
        return new FightUnitOwnership(
            teamId,
            participantId,
            controllerType);
    }
}