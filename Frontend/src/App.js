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
    this.majLivingCells = this.majLivingCells.bind(this);
  }

  majLivingCells(livingCellss)
  {
    this.setState({ livingCells : livingCellss });
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
        <div className="board">
          <CanvaCells livingCells={this.state.livingCells} height="20" width="20" cellSize="20" notifyParent={this.majLivingCells} />

          <BarLoader
            color={'#123abc'}
            loading={true}
          />
        </div>
      );
    } else
    {
      return (
        <div className="board">

          <CanvaCells livingCells={this.state.livingCells} height="20" width="20" cellSize="20" notifyParent={this.majLivingCells} />

          <div>
            <button onClick={this.buildHistory} disabled={this.state.games.length > 0 || this.state.livingCells.length == 0}>Build History</button>
            <button onClick={this.reset} disabled={this.state.games.length == 0}>Reset History</button>

            <label>Until round</label><input type="number" onChange={this.roundChange} value={this.state.lastRound} className="lastRound"/>
            <div>
              <button onClick={this.forwardInHistory} disabled={this.state.currentRound == 0 || this.state.games.length == 0}>Forward</button>
              <input type="range" min="0" max={this.state.lastRound} onChange={this.rangeChange} value={this.state.currentRound} disabled={this.state.games.length == 0} className="progressStep"/>
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


        </div>
      );
    }



  }
}

class SimpleGameOfLife extends Component {

  constructor(props) {
    super(props);
    this.state = { livingCells : [], round : 0, templatesFigure : []};
    this.nextRound = this.nextRound.bind(this);
    this.reset = this.reset.bind(this);
    this.saveFigure = this.saveFigure.bind(this);
    this.submitFile = this.submitFile.bind(this);
    this.majLivingCells = this.majLivingCells.bind(this);
  }

  majLivingCells(livingCellss)
  {
    this.setState({ livingCells : livingCellss });
  }

  nextRound()
  {
    Axios.post('http://localhost:50047/api/gameoflife/nextround', this.state)
    .then((response) => {
      this.setState({ livingCells : response.data.livingCells, currentRound : response.data.round});
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
    Axios.post('http://localhost:50047/api/gameoflife/readfigure', formData, {
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

  componentDidMount() {
    Axios.get('http://localhost:50047/api/gameoflife/AllFigures')
    .then((response) => {
      this.setState({ templatesFigure : response.data});
    });
   }

  render() {

    const figures =
    this.state.templatesFigure.map((figure) =>
        <CanvaFigure key={figure.ID} id={figure.ID} figureName={figure.figureName} relativeCoords={figure.template} />
      );
    return (
      <div className="board">
        <div>
          <div className="figures">
            {figures}
          </div>
          <CanvaCells livingCells={this.state.livingCells} height="20" width="20" cellSize="20" notifyParent={this.majLivingCells} />
          <div>
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

        </div>


      </div>
    );
  }
}

class CanvaFigure extends Component {
  constructor(props) {
    super(props);
    this.state = {id :  this.props.id, figureName : this.props.figureName, relativeCoords : this.props.relativeCoords};
  }

  render()
  {
    return (
      <div>
        {this.state.figureName}
        <canvas ref="fig" height="20" width="20" />
      </div>
    );
  }
}

class CanvaCells extends Component {
  constructor(props) {
    super(props);
    this.state =
    {
      livingCells : props.livingCells,
      height : props.height,
      width : props.width,
      cellSize : props.cellSize,
      mouseDown : false,
      mouseDownX : 0,
      mouseDownY : 0,
      drawOption : "draw"
    }
    this.switchCellState = this.switchCellState.bind(this);
    this.heightChange = this.heightChange.bind(this);
    this.widthChange = this.widthChange.bind(this);
    this.mouseUp = this.mouseUp.bind(this);
    this.mouseDown = this.mouseDown.bind(this);
    this.mouseMove = this.mouseMove.bind(this);
  }

  mouseDown(event){
    var position =  this.getMousePos(this.refs.canvas, event);
    var graphX = Math.trunc(position.x / this.state.cellSize );
    var graphY =  Math.trunc(position.y / this.state.cellSize);
    this.switchCellState(event);
    this.setState({mouseDown : true, mouseDownX : graphX, mouseDownY : graphY});
  }

  mouseUp(event){
    this.switchCellState(event);
    this.setState({mouseDown : false});
  }

  mouseMove(event){
    var position =  this.getMousePos(this.refs.canvas, event);
    var graphX = Math.trunc(position.x / this.state.cellSize );
    var graphY =  Math.trunc(position.y / this.state.cellSize);

    if (this.state.mouseDown && (graphX !== this.state.mouseDownX || graphY !== this.state.mouseDownY))
    {
      this.switchCellState(event);
      this.setState({mouseDownX : graphX, mouseDownY : graphY});
    }
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

   getMousePos(canvas, evt) {
    var rect = canvas.getBoundingClientRect();
    return {
      x: evt.clientX - rect.left,
      y: evt.clientY - rect.top
    };
}

  switchCellState(event)
  {
    const ctx = this.refs.canvas.getContext('2d');
    var position =  this.getMousePos(this.refs.canvas, event);
    var graphX = Math.trunc(position.x / this.state.cellSize );
    var graphY =  Math.trunc(position.y / this.state.cellSize);

    if (this.isCellLiving(graphX, graphY)) {
      ctx.fillStyle = '#DCDCDC';
      this.state.livingCells.splice(this.state.livingCells.findIndex(function(element) { return element.coordX === graphX && element.coordY === graphY; }), 1) ;
    } else {
      ctx.fillStyle = 'black';
      this.state.livingCells.push({coordX : graphX, coordY : graphY });
    }
    this.setState({ livingCells : this.state.livingCells});
    this.props.notifyParent(this.state.livingCells);
    ctx.fillRect(this.state.cellSize * graphX, this.state.cellSize * graphY, this.state.cellSize, this.state.cellSize);

  }

  isCellLiving(coordX, coordY) {
    return this.state.livingCells.findIndex(function(element) { return element.coordX === coordX && element.coordY === coordY; }) !== -1;
  }

  componentDidMount() {
       this.updateCanvas();
   }

   componentDidUpdate() {
       this.updateCanvas();
   }

   updateCanvas() {
        const ctx = this.refs.canvas.getContext('2d');

        for(var indexY = 0; indexY < this.state.height; indexY++)
        {
          for(var indexX = 0; indexX < this.state.width; indexX++)
          {
            if (this.isCellLiving(indexX, indexY)) {
              ctx.fillStyle = 'black';
            } else {
              ctx.fillStyle = '#DCDCDC';
            }
            ctx.fillRect(this.state.cellSize * indexX, this.state.cellSize * indexY, this.state.cellSize, this.state.cellSize);
          }
        }
    }

  render()
  {
    return (
      <div className="canvas">
      <canvas ref="canvas" width={this.state.width * this.state.cellSize} height={this.state.height * this.state.cellSize} onClick={this.switchCellState} onMouseDown={this.mouseDown} onMouseUp={this.mouseUp} onMouseMove={this.mouseMove}/>
        <p>
          <label>Height : </label><input type="number" onChange={this.heightChange} className="height" value={this.state.height} />
          <label>Width : </label><input type="number" onChange={this.widthChange} className="width" value={this.state.width}/>
        </p>
        </div>
      );
  }
}



export default App;
