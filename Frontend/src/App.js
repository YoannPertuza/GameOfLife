import React, { Component } from 'react';
import FileSaver from 'file-saver';
import Axios from 'axios';
import './App.css';


class App extends Component {

  constructor(props) {
    super(props);
    this.state = { livingCells : [], round : 0, height : 10, width : 10};
    this.switchCellState = this.switchCellState.bind(this);
    this.nextRound = this.nextRound.bind(this);
    this.reset = this.reset.bind(this);
    this.heightChange = this.heightChange.bind(this);
    this.widthChange = this.widthChange.bind(this);
    this.saveFigure = this.saveFigure.bind(this);
    this.submitFile = this.submitFile.bind(this);
  }

  switchCellState(coordx, coordy, isAlive)
  {
    if (isAlive) {
       this.state.livingCells.push({coordX : coordx, coordY : coordy });
    } else {
      this.state.livingCells.splice(this.state.livingCells.findIndex(function(element) { return element.coordX === coordx && element.coordY === coordy; }), 1) ;
    }

    this.setState({ livingCells : this.state.livingCells});
  }

  nextRound()
  {
    var state = this;

    Axios.post('http://gameoflifeapi-dev.us-west-2.elasticbeanstalk.com/api/gameoflife/nextround', this.state)
    .then(function (response) {
      state.setState({ livingCells : response.data.livingCells, round : response.data.round});
    });

  }

  heightChange(event)
  {
    this.setState({ livingCells : [], round : 0, height : event.target.value});
  }

  widthChange(event)
  {
    this.setState({ livingCells : [], round : 0, width : event.target.value});
  }

  reset()
  {
    this.setState({ livingCells : [], round : 0});
  }

  saveFigure()
  {
    var blob = new Blob([JSON.stringify(this.state.livingCells)], {type: "application/json;charset=utf-8"});
    FileSaver.saveAs(blob, "figure.json");
  }

  submitFile(event)
  {
    event.preventDefault();
    var state = this;
    var formData = new FormData();
    formData.append("file", this.fileInput.files[0]);
    Axios.post('http://gameoflifeapi-dev.us-west-2.elasticbeanstalk.com/api/gameoflife/uploadfigure', formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
    }).then(function (response) {
      state.setState({ livingCells : JSON.parse(response.data)});
    });;
  }

  isCellLiving(coordX, coordY) {
    return this.state.livingCells.findIndex(function(element) { return element.coordX === coordX && element.coordY === coordY; }) !== -1;
  }

  render() {

    var rows = [];
    for(var i=0; i < this.state.height; i++) {
      rows.push(i);
    }
    var colums = [];
    for(var j=0; j < this.state.width; j++) {
      colums.push(j);
    }



    return (
      <div className="App">
        {rows.map((row, indexY) =>
            <div className="row" key={indexY}>
              {colums.map((row, indexX) =>
                  <Cell
                      key={indexX * 10 + indexY}
                      switchState={this.switchCellState}
                      isAlive={this.isCellLiving(indexX, indexY)}
                      x={indexX}
                      y={indexY}
                  />
              )}
            </div>
          )}

      <button onClick={this.nextRound} >Next Round</button>
      <button onClick={this.reset} >Reset</button>
      <button onClick={this.saveFigure}>Save figure</button>

      <form onSubmit={this.submitFile}>
        <input name="figure" type="file" ref={input => {
              this.fileInput = input;
            }}/>
        <input type="submit" />
      </form>

      <label>Height : </label><input type="number" onChange={this.heightChange} />
      <label>Width : </label><input type="number" onChange={this.widthChange} />
      <h3>Round : {this.state.round}</h3>
      </div>
    );
  }
}

class Cell extends Component {

  constructor(props) {
    super(props);
    this.state = { over : false, isAlive : this.props.isAlive, coordX : this.props.x, coordY : this.props.y };
    this.switchState = this.switchState.bind(this);
    this.dragStart = this.dragStart.bind(this);
    this.drop = this.drop.bind(this);
  }

  componentWillReceiveProps(nextProps)
  {
      this.setState({ over : false, isAlive : nextProps.isAlive, coordX : nextProps.x, coordY : nextProps.y });
  }

  dragStart()
  {
    console.log(this.state);
  }

  drop(event)
  {

    console.log(event.screenX);
  }

  switchState()
  {
    if (this.state.isAlive) {
      this.setState({isAlive: false});
      this.props.switchState(this.state.coordX, this.state.coordY, false);
    } else {
      this.setState({isAlive: true});
      this.props.switchState(this.state.coordX, this.state.coordY, true);
    }
  }

  render() {
    if (this.state.isAlive) {
      return (
        <div className="alive-cell" onClick={this.switchState}  onDragStart={this.dragStart} onDragEnd={this.drop} />
      );
    } else {
      return (
        <div className="dead-cell" onClick={this.switchState} onDragStart={this.dragStart} onDragEnd={this.drop} />
      );
    }

  }
}

export default App;
