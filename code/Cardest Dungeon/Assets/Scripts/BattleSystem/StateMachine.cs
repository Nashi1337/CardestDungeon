using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    public enum States { Start, Standby, On };
    public States State { get; set; }

    public enum Events { PlugIn, TurnOn, TurnOff, RemovePower };

    private Action[,] fsm;

    public FiniteStateMachine()
    {
        this.fsm = new Action[3, 4] { 
                //PlugIn,       TurnOn,                 TurnOff,            RemovePower
                {this.PowerOn,  null,                   null,               null},              //start
                {null,          this.StandbyWhenOff,    null,               this.PowerOff},     //standby
                {null,          null,                   this.StandbyWhenOn, this.PowerOff} };   //on
    }
    public void ProcessEvent(Events theEvent)
    {
        this.fsm[(int)this.State, (int)theEvent].Invoke();
    }

    private void PowerOn() { this.State = States.Standby; }
    private void PowerOff() { this.State = States.Start; }
    private void StandbyWhenOn() { this.State = States.Standby; }
    private void StandbyWhenOff() { this.State = States.On; }
}