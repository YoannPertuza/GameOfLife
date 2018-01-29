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
    this.state = { livingCells : [], waiting : false, currentRound : 0, round : 0,  games : []};
    this.buildHistory = this.buildHistory.bind(this);
    this.nextInHistory = this.nextInHistory.bind(this);
    this.roundChange = this.roundChange.bind(this);

    this.rangeChange = this.rangeChange.bind(this);
  }


  buildHistory()
  {
    this.setState({waiting : true});
    Axios.post('http://localhost:50047/api/gameoflife/historizegames', this.state)
    .then((response) => {
      this.setState({ games : response.data, waiting : false});
    });
  }

  nextInHistory()
  {
      this.setState({ livingCells : this.state.games[this.state.currentRound+1].livingCells, currentRound : this.state.currentRound+1 });
  }

  rangeChange(event)
  {
    this.setState({ livingCells : this.state.games[event.target.value].livingCells, currentRound : event.target.value });
  }

  roundChange(event)
  {
    this.setState({round : event.target.value})
  }

  render()
  {

    if (this.state.waiting)
    {
      return (
        <div>
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
          <label>Until round</label><input type="number" onChange={this.roundChange} value={this.state.round}/>
          <button onClick={this.nextInHistory}>Next</button>
          <input type="range" min="0" max={this.state.round-1} onChange={this.rangeChange} value={this.state.currentRound}/>

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
    this.state = {livingCells : props.livingCells, height : 10, width : 10}
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
