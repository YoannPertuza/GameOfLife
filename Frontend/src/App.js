import React, { Component } from 'react';
import FileSaver from 'file-saver';
import { BarLoader } from 'react-spinners';
import Axios from 'axios';
import './App.css';

class App extends Component {
  constructor(props) {
    super(props);
  }

  render()
  {
    return (
      <div className="App">
        <SimpleGameOfLife />
        <HistoricalGameOfLifeName />
      </div>
    );
  }
}

class HistoricalGameOfLifeName extends Component {
  constructor(props) {
    super(props);
    this.state = { livingCells : [], waitingForHistory : false, currentRound : 0, lastRound : 10,  games : [], speed : 500, isPlaying : false};
    this.buildHistory = this.buildHistory.bind(this);
    this.nextInHistory = this.nextInHistory.bind(this);
    this.forwardInHistory = this.forwardInHistory.bind(this);
    this.roundChange = this.roundChange.bind(this);
    this.rangeChange = this.rangeChange.bind(this);
    this.reset = this.reset.bind(this);
    this.play = this.play.bind(this);
    this.speedChange = this.speedChange.bind(this);
    this.stop = this.stop.bind(this);
  }


  speedChange(event)
  {
    this.setState({ speed : event.target.value });
  }

  sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  reset()
  {
    this.setState({ livingCells : [], waitingForHistory : false, currentRound : 0, lastRound : 10,  games : []});
  }

  buildHistory()
  {
    this.setState({waitingForHistory : true});
    Axios.post('http://localhost:50047/api/gameoflife/historizegames', { livingCells :  this.state.livingCells, currentRound : this.state.currentRound, lastRound : this.state.lastRound} )
    .then((response) => {
      this.setState({ games : response.data, waitingForHistory : false});
    });
  }

  nextInHistory()
  {
      this.setState({ livingCells : this.state.games[++this.state.currentRound].livingCells, currentRound : this.state.currentRound++ });
  }

  forwardInHistory()
  {
      this.setState({ livingCells : this.state.games[--this.state.currentRound].livingCells, currentRound : this.state.currentRound-- });
  }

  rangeChange(event)
  {
    this.setState({ livingCells : this.state.games[event.target.value].livingCells, currentRound : event.target.value });
  }

  roundChange(event)
  {
    this.setState({lastRound : event.target.value});
  }

  async play(event)
  {

    for(var index = this.state.currentRound; index <= this.state.lastRound; index++)
    {
        this.setState({ livingCells : this.state.games[index].livingCells, currentRound : index });
        await this.sleep(this.state.speed);
    }

  }

  stop(event)
  {
    this.setState({isPlaying : false})
  }

  render()
  {

    if (this.state.waitingForHistory)
    {
      return (
        <div>
          <Cells livingCells={this.state.livingCells} />

          <BarLoader
            color={'#123abc'}
            loading={true}
          />
        </div>
      );
    } else
    {
      return (
        <div>
          <Cells livingCells={this.state.livingCells} />

          <button onClick={this.buildHistory}>Build History</button>
          <button onClick={this.reset} >Reset History</button>

          <label>Until round</label><input type="number" onChange={this.roundChange} value={this.state.lastRound}/>
          <div>
            <button onClick={this.forwardInHistory} disabled={this.state.currentRound == 0 || this.state.games.length == 0}>Forward</button>
            <input type="range" min="0" max={this.state.lastRound} onChange={this.rangeChange} value={this.state.currentRound}/>
            <button onClick={this.nextInHistory} disabled={this.state.currentRound == this.state.lastRound || this.state.games.length == 0}>Next</button>
            <button onClick={this.play} disabled={this.state.games.length == 0}>Play</button>
            <select onChange={this.speedChange} value={this.state.speed}>
              <option value="1000">Low</option>
              <option value="500">Medium</option>
              <option value="250">Fast</option>
              <option value="125">Super Fast</option>
            </select>
          </div>
          <h3>Round : {this.state.currentRound}</h3>
        </div>
      );
    }



  }
}

class SimpleGameOfLife extends Component {

  constructor(props) {
    super(props);
    this.state = { livingCells : [], round : 0};
    this.nextRound = this.nextRound.bind(this);
    this.reset = this.reset.bind(this);
    this.saveFigure = this.saveFigure.bind(this);
    this.submitFile = this.submitFile.bind(this);
  }

  nextRound()
  {
    Axios.post('http://gameoflifeapi-dev.us-west-2.elasticbeanstalk.com/api/gameoflife/nextround', this.state)
    .then((response) => {
      this.setState({ livingCells : response.data.livingCells, round : response.data.round});
    });
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
    var formData = new FormData();
    formData.append("file", this.fileInput.files[0]);
    Axios.post('http://gameoflifeapi-dev.us-west-2.elasticbeanstalk.com/api/gameoflife/readfigure', formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
    }).then((response) => {
      this.setState({ livingCells : JSON.parse(response.data)});
    });
  }

  isCellLiving(coordX, coordY) {
    return this.state.livingCells.findIndex(function(element) { return element.coordX === coordX && element.coordY === coordY; }) !== -1;
  }

  render() {

    return (
      <div>
        <Cells livingCells={this.state.livingCells} />

        <button onClick={this.nextRound} >Next Round</button>
        <button onClick={this.reset} >Reset</button>
        <button onClick={this.saveFigure}>Save figure</button>

        <form onSubmit={this.submitFile}>
          <input name="figure" type="file" ref={input => {
                this.fileInput = input;
              }}/>
          <input type="submit" />
        </form>

        <h3>Round : {this.state.round}</h3>
      </div>
    );
  }
}


class Cells extends Component {
  constructor(props) {
    super(props);
    this.state = {livingCells : props.livingCells, height : 20, width : 20}
    this.switchCellState = this.switchCellState.bind(this);
    this.heightChange = this.heightChange.bind(this);
    this.widthChange = this.widthChange.bind(this);
  }

  componentWillReceiveProps(nextProps)
  {
      this.setState({ livingCells : nextProps.livingCells });
  }

  heightChange(event)
  {
    this.setState({ height : event.target.value});
  }

  widthChange(event)
  {
    this.setState({ width : event.target.value});
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

  isCellLiving(coordX, coordY) {
    return this.state.livingCells.findIndex(function(element) { return element.coordX === coordX && element.coordY === coordY; }) !== -1;
  }

  render()
  {
    var rows = [];
    for(var i=0; i < this.state.height; i++) {
      rows.push(i);
    }
    var colums = [];
    for(var j=0; j < this.state.width; j++) {
      colums.push(j);
    }

    return (
      <div>
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
          )
        }
        <label>Height : </label><input type="number" onChange={this.heightChange} />
        <label>Width : </label><input type="number" onChange={this.widthChange} />
        </div>
      );
  }
}


class Cell extends Component {

  constructor(props) {
    super(props);
    this.state = { mouseDown : false, isAlive : this.props.isAlive, coordX : this.props.x, coordY : this.props.y };
    this.switchState = this.switchState.bind(this);
  }

  componentWillReceiveProps(nextProps)
  {
      this.setState({ mouseDown : false, isAlive : nextProps.isAlive, coordX : nextProps.x, coordY : nextProps.y });
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
        <div className="alive-cell" onClick={this.switchState}   />
      );
    } else {
      return (
        <div className="dead-cell" onClick={this.switchState}  />
      );
    }

  }
}

export default App;
