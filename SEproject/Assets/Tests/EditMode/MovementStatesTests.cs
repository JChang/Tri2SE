// Christopher Chang

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MovementStates
{
    private GameObject deerObject;
    private DeerStateMachine deerStateMachine;
    public LayerMask groundLayer = LayerMask.GetMask("groundLayer");

    [SetUp]
    public void SetUp()
    {
        deerObject = new GameObject();
        deerStateMachine = deerObject.AddComponent<DeerStateMachine>();
        deerStateMachine.rb = deerObject.AddComponent<Rigidbody>();
        deerStateMachine.t = deerObject.transform;
        deerStateMachine.animator = deerObject.AddComponent<Animator>();
        deerStateMachine.IsGrounded = true;
        var deerModel = new GameObject("Deer_001");
        deerModel.transform.parent = deerObject.transform;
        deerStateMachine.animator = deerModel.AddComponent<Animator>();
        deerStateMachine.setState(new DeerWalk(deerStateMachine)); 
        
    }

    [TearDown]
    public void Teardown()
    {
        GameObject.DestroyImmediate(deerObject);
    }

    [Test]
    public void InitialStateIsDeerStateMachine()
    {
        Assert.IsInstanceOf(typeof(DeerStateMachine), deerStateMachine);
    }

    [Test]
    public void InitialStateIsDeerWalk()
    {
        Assert.IsInstanceOf(typeof(DeerWalk), deerStateMachine.GetCurrentState());
    }

    [Test]
    public void CannotTransitionFromWalkJumpToSprint()
    {
        var jumpState = new DeerJump(deerStateMachine, 2f);
        deerStateMachine.setState(jumpState);
        deerStateMachine.IsGrounded = true;
        deerStateMachine.deerState.advanceState();
        Assert.IsNotInstanceOf(typeof(DeerSprint), deerStateMachine.GetCurrentState());
    }

    [Test]
    public void CannotTransitionFromJumpToWalkWhileAirborne()
    {
        var jumpState = new DeerJump(deerStateMachine, 2f);
        deerStateMachine.setState(jumpState);
        deerStateMachine.IsGrounded = false;
        deerStateMachine.deerState.advanceState();
        Assert.IsNotInstanceOf(typeof(DeerWalk), deerStateMachine.GetCurrentState());

    }

    [Test]
    public void CannotTransitionFromSprintJumpToWalk()
    {
        var jumpState = new DeerJump(deerStateMachine, 4f);
        deerStateMachine.setState(jumpState);
        deerStateMachine.IsGrounded = true;
        deerStateMachine.deerState.advanceState();
        Assert.IsNotInstanceOf(typeof(DeerWalk), deerStateMachine.GetCurrentState());
    }

    [Test]
    public void CannotTransitionFromJumpToSprintWhileAirborne()
    {
        var jumpState = new DeerJump(deerStateMachine, 4f);
        deerStateMachine.setState(jumpState);
        deerStateMachine.IsGrounded = false;
        deerStateMachine.deerState.advanceState();
        Assert.IsNotInstanceOf(typeof(DeerSprint), deerStateMachine.GetCurrentState());
    }

    [Test]
    public void CanTransitionFromWalkToSprint()
    {
        deerStateMachine.setState(new DeerWalk(deerStateMachine));
        deerStateMachine.deerState.handleShift();
        Assert.IsInstanceOf(typeof(DeerSprint), deerStateMachine.GetCurrentState());
    }

    [Test]
    public void CanTransitionFromJumpToWalk()
    {
        var jumpState = new DeerJump(deerStateMachine, 2f);
        deerStateMachine.setState(jumpState);
        deerStateMachine.IsGrounded = true;
        deerStateMachine.deerState.advanceState();
        Assert.IsInstanceOf(typeof(DeerWalk), deerStateMachine.GetCurrentState());
    }

    [Test]
    public void CanTransitionFromJumpToSprint()
    {
        var jumpState = new DeerJump(deerStateMachine, 4f);
        deerStateMachine.setState(jumpState);
        deerStateMachine.IsGrounded = true;
        deerStateMachine.deerState.advanceState();
        Assert.IsInstanceOf(typeof(DeerSprint), deerStateMachine.GetCurrentState());
    }
}
