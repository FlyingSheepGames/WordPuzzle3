
// Command to handle node babel build:
// npx babel main.js --presets @babel/preset-react --plugins @babel/plugin-proposal-class-properties --watch --out-file main-compiled.js
// npx babel main.js --presets @babel/preset-react --presets minify --plugins @babel/plugin-proposal-class-properties --out-file main-compiled.js


class PuzzlePiece extends React.Component 
{
	constructor(props) {
		super(props);
		this.state = {value: ""};
		this.handleChange = this.handleChange.bind(this);
		this.handleKeyDown = this.handleKeyDown.bind(this);
		this.handleFocus = this.handleFocus.bind(this);
		this.handleBlur = this.handleBlur.bind(this);
		this.getMyIndex = this.getMyIndex.bind(this);
		this.updateHookBuddy = this.updateHookBuddy.bind(this);
		this.myRef = React.createRef();
	}

	getMyIndex() {
		// Change focus here.
		var elements = document.querySelectorAll('div.puzzle.visible input.puzzle-piece.checkanswers');
		var element_index = -1;

		for (var ind = 0; ind < elements.length; ind ++)
		{
			if (elements[ind] == this.myRef.current)
			{
				element_index = ind;
				break;
			}	
		}

		return [element_index, elements];
	}

	updateHookBuddy(val) {
		var elements = document.querySelectorAll('div.puzzle.visible input.puzzle-piece.puzzle-piece-' + this.props.hook);
		for (var ind = 0 ; ind < elements.length ; ind ++)
		{
			var element = elements[ind];
			if (element == this.myRef.current) continue;
			if (element.value != val)
			{
				element.value = val;
			}
		}
	}

	handleKeyDown(e) {
		

		var letter = e.target.value.slice(-1);
		var key = event.keyCode || event.charCode;

		// Change focus here.
		var values = this.getMyIndex();
		var element_index = values[0];
		var elements = values[1];

		if (key == 8)
		{
			e.stopPropagation();
			e.preventDefault();
			// On backspace, check if value is empty.
			if (letter != null && letter != "" && !e.target.readOnly)
			{
				this.setState({value: ""});
				this.myRef.current.value = "";
				this.updateHookBuddy("");

				e.target.parentElement.dataset["bakedin"] = "false";
				e.target.parentElement.classList.remove("shaded");
			}

			// Tab backward.
			element_index -= 1;
			element_index += elements.length;
			element_index %= elements.length;

			elements[element_index].focus()

		}
	}

	handleFocus(e) {
		console.log("handleFocus");
		var elements = document.querySelectorAll('div.puzzle.visible input.puzzle-piece.puzzle-piece-' + this.props.hook);
		for (var ind = 0 ; ind < elements.length ; ind ++)
		{
			var element = elements[ind];
			if (element == this.myRef.current) continue;
			element.classList.add("focused");
		}
	}

	handleBlur(e) {
		var elements = document.querySelectorAll('div.puzzle.visible input.puzzle-piece.puzzle-piece-' + this.props.hook);
		for (var ind = 0 ; ind < elements.length ; ind ++)
		{
			var element = elements[ind];
			if (element == this.myRef.current) continue;
			element.classList.remove("focused");
		}
	}

	handleChange(e) {
		var letter = e.target.value.slice(-1);
		if (letter != null && letter != "") 
		{
			// Return here if the input is not actually a letter.
			if (! /^[a-zA-Z]$/.test(letter))
			{
				return;
			}

			letter = letter.toUpperCase();
		}
		this.setState({value: letter});
		this.myRef.current.value = letter;

		this.updateHookBuddy(letter);

		// Change focus here.
		var values = this.getMyIndex();
		var element_index = values[0];
		var elements = values[1];

		var are_all_correct = this.props.check_puzzle();

		if (letter != null && letter != "" && !are_all_correct) 
		{

			// Tab forward.
			element_index += 1;
			element_index += elements.length;
			element_index %= elements.length;

			elements[element_index].focus();
		}
		
	}


	render(){
		return (
		<input
			ref={this.myRef}
			className={"puzzle-piece " + "puzzle-piece-" + this.props.hook + " " + (this.props.checkAnswers == "true" ? "checkanswers" : "")}
			type="text"
			value={this.state.value}
			readOnly={this.props.readOnly == "true"}
			onKeyDown={this.handleKeyDown}
			onChange={this.handleChange}
			onFocus={this.handleFocus}
			onBlur={this.handleBlur}
			value={this.props.value}
		/>
		);	
	}
}

class Puzzle extends React.Component 
{
	constructor(props) {
		super(props);
		this.state = {"number_of_letters": 1, "checking_answers": 0, "correct_answers": ""}
		this.check_answers = this.check_answers.bind(this);
		this.reveal_letter = this.reveal_letter.bind(this);
		this.common_render = this.common_render.bind(this);
		this.toggleShading = this.toggleShading.bind(this);
		this.are_all_correct = this.are_all_correct.bind(this);
		this.check_puzzle = this.check_puzzle.bind(this);
	}

	toggleShading(e) {
		var target = e.target;

		if (target.tagName == "TD")
		{
			target = target.parentNode;
		}

		if (target.classList.contains("shaded"))
		{
			target.classList.remove("shaded");
		}
		else
		{
			target.classList.add("shaded");
		}
	}

	common_render(fragment=false) {
		return (
			<div className="puzzle-header row d-flex justify-content-center py-3 border-bottom">
				<div className="d-flex col-8 align-self-start">
					<h2>{this.props.data.name}</h2>
				</div>
				<div className="col-4 align-items-end d-flex">
					{}
					{!fragment && <button className="btn btn-primary" onClick={this.reveal_letter} type="button">Reveal a Letter</button>}
					<button className="btn btn-primary" onClick={this.check_answers} type="button">Check My Answers</button>		
				</div>
			</div>
		);
	}

	reveal_letter() {
		var elements = document.querySelectorAll('div.puzzle.visible input.puzzle-piece.checkanswers');


		for (var ind = 0; ind < elements.length; ind ++)
		{
			if (elements[ind].value != this.state.correct_answers[ind])
			{
				var el = elements[ind];
				var t = this.state.correct_answers[ind];

				el.value = t;

				el.classList.add("shaded-orange");

				this.check_puzzle();

				var hook = undefined;
				for (var i = 0 ; i < el.classList.length; i++)
				{
					if (el.classList[i].startsWith('puzzle-piece-'))
					{
						hook = el.classList[i];
						break;
					}
				}

				var elements2 = document.querySelectorAll('div.puzzle.visible input.puzzle-piece.' + hook);
				for (var ind = 0 ; ind < elements2.length ; ind ++)
				{
					var element = elements2[ind];
					if (element == el) continue;
					if (element.value != t)
					{
						element.value = t;
					}
				}
				return;
			}
		}
	}

	// Called automatically on button presses
	check_puzzle() {
		var result = this.are_all_correct();
		// Replace text for correction here.
		if (result[0])
		{
			var tdElements = document.getElementsByTagName("td");
			var found;

			for (var i = 0; i < tdElements.length; i++) {
				if (tdElements[i].textContent.toLowerCase() == ("(Solve " + this.props.data.name + ")").toLowerCase()) {
					found = tdElements[i];
					found.textContent = this.props.data.final_answer.toUpperCase();
					break;
				}
			}

			var quoteElement = document.getElementById("puzzle-quote");
			
			var searchMask = "\\(Solve " + this.props.data.name + "\\)";
			var regEx = new RegExp(searchMask, "ig");
			var replaceMask = this.props.data.final_answer.toUpperCase();

			quoteElement.textContent = quoteElement.textContent.replace(regEx , replaceMask);

			this.check_answers();
		}

		return result[0];
	}

	are_all_correct() {
		var allCorrect = true;
		var elements = document.querySelectorAll('div.puzzle.visible input.puzzle-piece.checkanswers');

		var incorrect = [];
		var correct = [];

		for (var ind = 0; ind < elements.length; ind ++)
		{
			if (elements[ind].value != this.state.correct_answers[ind])
			{
				allCorrect = false;
				if (elements[ind].value != "")
				{
					incorrect.push(elements[ind]);
				}
			}
			else
			{
				correct.push(elements[ind]);
			}
		}

		return [allCorrect, correct, incorrect]
	}

	check_answers() {
		if (this.state.checking_answers == 1) return;

		var result = this.are_all_correct();
		var allCorrect = result[0];
		var correct = result[1];
		var incorrect = result[2];

		for (var ind = 0; ind < correct.length; ind ++)
		{
			correct[ind].classList.add("correct");
			if (allCorrect)
			{
				correct[ind].readOnly = true;
				correct[ind].blur();
			}
		}

		for (var ind = 0; ind < incorrect.length; ind ++)
		{
			incorrect[ind].classList.add("incorrect");
		}

		if (!allCorrect)
		{
			this.state.checking_answers = 1;

			setTimeout(function(){
				
				var elements = document.querySelectorAll('div.puzzle input.puzzle-piece.checkanswers');
				for (var ind = 0; ind < elements.length; ind ++)
				{
					var element = elements[ind];

					if (element.classList.contains("correct") || element.classList.contains("incorrect"))
					{
						element.classList.add("checked-answer");
					}

					element.classList.remove("correct");
					element.classList.remove("incorrect");


				}
				setTimeout(function(){

					this.state.checking_answers = 0;
					
					var elements = document.querySelectorAll('div.puzzle input.puzzle-piece.checkanswers');
					for (var ind = 0; ind < elements.length; ind ++)
					{
						var element = elements[ind];

						element.classList.remove("checked-answer");
					}
				}.bind(this), 1000);
			}.bind(this), 2500);
		}
	}
}

class Puzzle_Box_Sum extends Puzzle
{
	constructor(props) {
		super(props);
		
		for (var ind = 0; ind < this.props.data.answers.length; ind ++)
		{
			this.state.correct_answers += this.props.data.answers[ind].toUpperCase();
		}

		this.state.correct_answers += this.props.data.final_answer.replace(/\s/g, '').toUpperCase();

	}

	render() {
		return <div className={"container puzzle " + this.props.visible + " " + this.props.puzzleId}>
			{this.common_render()}
			
			<div className="row">
				<div className="col">
					<h3 className="pb-4 border-bottom">{this.props.data.directions}</h3>
					
					
				</div>
			</div>
			<div className="row pt-2">
				<div className="col-12 col-sm-12 col-md-12 col-lg-6">
					<h2>List of Clues</h2>
					<table className="box-traversal-table">
						<tbody>
							{
								this.props.data.clues.map((clue, outer_index) => (
									<tr key={this.props.data.name+"tr"+outer_index} onClick={this.toggleShading}>
										<td className="box-traversal-clue-td" key={this.props.data.name+"td"+outer_index}>{clue.value}</td>
										<td className="box-traversal-clue-td" key={this.props.data.name+"td2"+outer_index}>{clue.clue}</td>
									</tr>
								))
							}
						</tbody>
					</table>

				</div>
				<div className="col-12 col-sm-12 col-md-12 col-lg-6">
					<h2>List of Words</h2>
					<table className="box-traversal-table">
						<tbody>
							{
								this.props.data.solution_lengths.map((length, outer_index) => (
									<tr key={this.props.data.name+"tr"+outer_index}>
										<td className="box-traversal-clue-td" key={this.props.data.name+"td"+outer_index}>{length}</td>
										{
											Array.from(this.props.data.answers[outer_index]).map((letter, inner_index) => (
												<td className="box-sum-td" key={this.props.data.name+"td"+outer_index+inner_index}>
													<div key={this.props.data.name+"div"+outer_index+inner_index}>
														<PuzzlePiece key={this.props.data.name + "p" + outer_index + inner_index} puzzleId={this.props.puzzleId} hook={this.props.puzzleId + String.fromCharCode(65 + outer_index) + this.state.number_of_letters++} readOnly="false" checkAnswers="true" check_puzzle={this.check_puzzle} />
													</div>
												</td>
											))
										}
									</tr>
								))
							}
						</tbody>
					</table>
					<h3 className="pt-4">Solution</h3>
					{
						Array.from(this.props.data.solution_boxes).map((solution,index) => {
							return <PuzzlePiece key={this.props.data.name + "sol" + index} puzzleId={this.props.puzzleId} readOnly="false" checkAnswers="true" hook={this.props.puzzleId + solution} check_puzzle={this.check_puzzle}/>
						})
					}
				</div>
			</div>
		</div>;
	}

}


class Puzzle_Box_Simple extends Puzzle
{
	constructor(props) {
		super(props);
		
		for (var ind = 0; ind < this.props.data.clues.length; ind ++)
		{
			this.state.correct_answers += this.props.data.clues[ind].answer.toUpperCase();
		}

		this.state.correct_answers += this.props.data.final_answer.replace(/\s/g, '').toUpperCase();

	}

	render() {
		return <div className={"container puzzle " + this.props.visible + " " + this.props.puzzleId}>
			{this.common_render()}
			
			<div className="row">
				<div className="py-4 col-12 col-sm-12 col-md-12 col-lg-12 border-bottom">
					<h3 className="pb-4 border-bottom">{this.props.data.directions}</h3>
					<h2 className="py-2">Missing Words</h2>
					<div className="d-flex align-items-center justify-content-center drag_end">
						<table className="box-traversal-table">
							<tbody>
								{
									this.props.data.clues.map((clue, outer_index) => (
										<tr key={this.props.data.name+"tr"+outer_index}>
											<td className="box-traversal-clue-td" key={this.props.data.name+"td"+outer_index}>{clue.clue}</td>
											{
												Array.from(clue.answer).map((letter, inner_index) => (
													<td className={"box-sum-td " + ((inner_index + 1 == this.props.data.solution_column) ? "shaded-light" : "") } key={this.props.data.name+"td"+outer_index+inner_index}>
														<div key={this.props.data.name+"div"+outer_index+inner_index}>
															<PuzzlePiece key={this.props.data.name + "p" + outer_index + inner_index} puzzleId={this.props.puzzleId} hook={this.props.puzzleId + String.fromCharCode(65 + outer_index) + this.state.number_of_letters++} readOnly="false" checkAnswers="true" check_puzzle={this.check_puzzle}/>
														</div>
													</td>
												))
											}
										</tr>
									))
								}
							</tbody>
						</table>
					</div>
				</div>
				<div className="col-12 col-sm-12 col-md-12 col-lg-12">
					<h3 className="py-4 border-bottom">Solution</h3>
					{
						Array.from(this.props.data.final_answer).map((character,index) => {
							return <PuzzlePiece key={this.props.data.name + "sol" + index} puzzleId={this.props.puzzleId} readOnly="true" hook={this.props.puzzleId + String.fromCharCode(65 + index) + (this.props.data.solution_column + this.props.data.clues[0].answer.length * index).toString()} check_puzzle={this.check_puzzle}/>
						})
					}
				</div>
			</div>
		</div>;
	}

}


class Puzzle_Fragment extends Puzzle
{
	constructor(props) {
		super(props);

		for (var ind = 0; ind < this.props.data.clues.length; ind ++)
		{
			this.state.correct_answers += this.props.data.clues[ind].answer.toUpperCase();
		}

		this.state.correct_answers += this.props.data.final_answer.replace(/\s/g, '').toUpperCase();

		this.handleDragStart = this.handleDragStart.bind(this);
		this.handleDragDrop = this.handleDragDrop.bind(this);
		this.handleDragOver = this.handleDragOver.bind(this);
		this.handleDragEnter = this.handleDragEnter.bind(this);
		this.handleDragExit = this.handleDragExit.bind(this);
		this.handleDragEnd = this.handleDragEnd.bind(this);

		this.fragmentToggleShading = this.fragmentToggleShading.bind(this);
	}


	handleDragStart = (e) => {

		if (e.target.classList.contains("shaded"))
		{
			e.preventDefault();
			return;
		}

		e.dataTransfer.setData(encodeUpperCase(JSON.stringify(e.target.innerText + "::" + e.target.id)), '');
		e.target.classList.add("shaded-light");

	}

	handleDragEnd = (e) => {

		e.target.classList.remove("shaded-light");

		if (e.dataTransfer.dropEffect == "move"  || e.dataTransfer.dropEffect == "copy")
		{
			// Dropped successfully.
			e.target.classList.add("shaded");
			var all_good = this.check_puzzle();

			if (all_good)
			{
				var elements = document.querySelectorAll('div.puzzle.visible div.fragment-display div');
				for (var i = 0 ; i < elements.length; i++)
				{
					var el = elements[i];
					el.classList.add('disable-pointer');
				}
			}
		}


	}

	handleDragDrop = (e) => {
		e.stopPropagation();
		e.preventDefault();

		var et;
		if (e.target.nodeName == "INPUT")
		{
			et = e.target.parentNode;
		}
		else 
		{
			et = e.target;
		}


		// Drop Event
		var validTarget = true;

		var dt = JSON.parse(decodeUpperCase(e.dataTransfer.types[0]));
		var dt_parts = dt.split("::");
		var letters = dt_parts[0].split("");
		var elID = dt_parts[1];

		

		var e2 = document.getElementById(et.dataset["idNext"]);
		var e3 = null;
		if (e2 == null)
		{
			validTarget = false;
		}
		else
		{
			if (letters.length == 3)
			{
				e3 = document.getElementById(e2.dataset["idNext"]);
			}

			if (letters.length == 3 && e3 == null)
			{
				validTarget = false;
			}
		}

		if (validTarget)
		{
			if (et.dataset["bakedin"] == "true" || e2.dataset["bakedin"] == "true" || (e3 != null && e3.dataset["bakedin"] == "true"))
			{
				return;
			}

			et.dataset["bakedin"] = "true";
			e2.dataset["bakedin"] = "true";

			if (e3 != null)
			{
				e3.dataset["bakedin"] = "true";
			}

			et.classList.remove("shaded");
			e2.classList.remove("shaded");
			if (e3 != null)
			{
				e3.classList.remove("shaded");
			}

			et.dataset["sourceid"] = elID;
			
		}
	}

	handleDragOver = (e) => {
		e.stopPropagation();
		e.preventDefault();
	}

	handleDragEnter = (e) => {
		e.stopPropagation();
		e.preventDefault();
		if (e.target.nodeName == "DIV") // && e.relatedTarget.tagName != "INPUT")
		{

			// Drag Enter Event
			var validTarget = true;
			var dt = JSON.parse(decodeUpperCase(e.dataTransfer.types[0]));
			var dt_parts = dt.split("::");
			var letters = dt_parts[0].split("");
			var elID = dt_parts[1];

			console.log(letters);

			var e2 = document.getElementById(e.target.dataset["idNext"]);
			var e3 = null;
			if (e2 == null)
			{
				validTarget = false;
			}
			else
			{
				if (letters.length == 3)
				{
					e3 = document.getElementById(e2.dataset["idNext"]);
				}

				if (letters.length == 3 && e3 == null)
				{
					validTarget = false;
				}
			}

			if (validTarget)
			{
				if (e.target.dataset["bakedin"] == "true" || e2.dataset["bakedin"] == "true" || (e3 != null && e3.dataset["bakedin"] == "true"))
				{
					return;
				}


				updateCharacter(e.target, letters[0]);
				updateCharacter(e2, letters[1]);
				updateCharacter(e3, letters[2]);

				e.target.classList.add("shaded");
				e2.classList.add("shaded");
				if (e3 != null)
				{
					e3.classList.add("shaded");
				}

			}
		}
	}

	handleDragExit = (e) => {
		e.stopPropagation();
		e.preventDefault();

		if (e.target.nodeName == "DIV") // && e.relatedTarget.tagName != "INPUT")
		{

			// Drag Exit Event
			var validTarget = true;
			var dt = JSON.parse(decodeUpperCase(e.dataTransfer.types[0]));
			var dt_parts = dt.split("::");
			var letters = dt_parts[0].split("");
			var elID = dt_parts[1];
			var e2 = document.getElementById(e.target.dataset["idNext"]);
			var e3 = null;
			if (e2 == null)
			{
				validTarget = false;
			}
			else
			{
				if (letters.length == 3)
				{
					e3 = document.getElementById(e2.dataset["idNext"]);
				}

				if (letters.length == 3 && e3 == null)
				{
					validTarget = false;
				}
			}

			if (validTarget)
			{
				if (e.target.dataset["bakedin"] == "true" || e2.dataset["bakedin"] == "true" || (e3 != null && e3.dataset["bakedin"] == "true"))
				{
					return;
				}

				updateCharacter(e.target, "");
				updateCharacter(e2, "");
				updateCharacter(e3, "");

				e.target.classList.remove("shaded");
				e2.classList.remove("shaded");

				if (e3 != null)
				{
					e3.classList.remove("shaded");
				}

			}

		}

	}

	fragmentToggleShading(e)
	{
		console.log("fragmentToggleShading start");

		//console.log(e);

		var target = e.target;
		if (target.nodeName == "SPAN")
		{
			target = target.parentNode;
		}

		if (!target.classList.contains("shaded"))
		{
			//console.log("fragmentToggleShading not shaded, returning...");

			// Fragment Toggle Shading
			var validTarget = true;
			
			var letters = e.target.innerText.split("");

			// Find an unclaimed textbox here.
			var et = undefined;
			
			var elements = document.querySelectorAll('div.puzzle.visible div.fragment-div');
			for (var i = 0; i < elements.length; i++)
			{
				var _element = elements[i];
				if (_element.dataset["bakedin"] != "true")
				{
					et = _element;
					break;
				}
			}

			if (et == undefined)
			{
				return;
			}

			var e2 = document.getElementById(et.dataset["idNext"]);
			var e3 = null;
			if (e2 == null)
			{
				validTarget = false;
			}
			else
			{
				if (letters.length == 3)
				{
					e3 = document.getElementById(e2.dataset["idNext"]);
				}

				if (letters.length == 3 && e3 == null)
				{
					validTarget = false;
				}
			}


			if (validTarget)
			{

				if (e.target.dataset["bakedin"] == "true" || e2.dataset["bakedin"] == "true" || (e3 != null && e3.dataset["bakedin"] == "true"))
				{
					return;
				}

				updateCharacter(et, letters[0]);
				updateCharacter(e2, letters[1]);
				updateCharacter(e3, letters[2]);

				// et.classList.add("shaded");
				// e2.classList.add("shaded");
				// if (e3 != null)
				// {
				// 	e3.classList.add("shaded");
				// }

				et.dataset["bakedin"] = "true";
				e2.dataset["bakedin"] = "true";

				if (e3 != null)
				{
					e3.dataset["bakedin"] = "true";
				}

				et.dataset["sourceid"] = target.id;

				// Dropped successfully.
				target.classList.add("shaded");
				var all_good = this.check_puzzle();

				if (all_good)
				{
					var elements = document.querySelectorAll('div.puzzle.visible div.fragment-display div');
					for (var i = 0 ; i < elements.length; i++)
					{
						var el = elements[i];
						el.classList.add('disable-pointer');
					}
				}


			}

			return;
		}

		// Find the corresponding boxes to empty.
		var el = document.querySelector('div.fragment-div[data-sourceid="' + target.id + '"]');

		console.log("looking for : " + 'div.fragment-div[data-sourceid="' + target.id + '"]');

		if (el != null)
		{

			console.log("fragmentToggleShading found el");

			updateCharacter(el, "");
			el.dataset["bakedin"] = "false";
			el.classList.remove("shaded");
			var e2 = document.getElementById(el.dataset["idNext"]);
			updateCharacter(e2, "");
			e2.dataset["bakedin"] = "false";
			e2.classList.remove("shaded");
			if (target.innerText.length == 3)
			{
				var e3 = document.getElementById(e2.dataset["idNext"]);
				updateCharacter(e3, "");
				e3.dataset["bakedin"] = "false";
				e3.classList.remove("shaded");
			}
			el.dataset['sourceid'] = "";			
		}
		else
		{
			console.log("fragmentToggleShading did not find el");
		}

		target.classList.remove("shaded");

	}


	render() {
		return <div className={"container puzzle " + this.props.visible + " " + this.props.puzzleId}>
			{this.common_render(true)}
			
			<div className="row py-4">
				<h3 className="pb-4 border-bottom">{this.props.data.directions}</h3>
				<h2 className="pb-2">Missing Words</h2>
				<div className="pb-4 border-bottom d-flex align-items-center justify-content-center">
					<table className="box-traversal-table">
						<tbody>
							{
								this.props.data.clues.map((clue, outer_index) => (
									<tr key={this.props.data.name+"tr"+outer_index}>
										<td className="box-traversal-clue-td" key={this.props.data.name+"td"+outer_index}>{clue.clue}</td>
										{
											Array.from(clue.answer).map((letter, inner_index) => (
												<td className="box-fragment-td" key={this.props.data.name+"td"+outer_index+inner_index}>
													<div className="fragment-div" onDrop={this.handleDragDrop} onDragEnter={this.handleDragEnter} onDragLeave={this.handleDragExit} onDragOver={this.handleDragOver} key={this.props.data.name+"div"+outer_index+inner_index} id={this.props.data.name+"div"+outer_index+inner_index} data-id-next={this.props.data.name+"div"+outer_index+(inner_index+1)} data-bakedin="false" data-hook={this.props.puzzleId + String.fromCharCode(65 + outer_index) + this.state.number_of_letters}>
														<PuzzlePiece key={this.props.data.name + "p" + outer_index + inner_index} puzzleId={this.props.puzzleId} hook={this.props.puzzleId + String.fromCharCode(65 + outer_index) + this.state.number_of_letters++} readOnly="true" checkAnswers="true" check_puzzle={this.check_puzzle}/>
													</div>
												</td>
											))
										}
									</tr>
								))
							}
						</tbody>
					</table>
				</div>
			</div>
			<div className="row pt-2 pb-4">
				<div className="col-12 col-sm-12 col-md-12 col-lg-6">
					<h2 className="pb-4 border-bottom">Fragments</h2>
					<h3 className="pt-2">Length 2:</h3>
					<div className="fragment-display d-flex align-items-center justify-content-center text-center">
					{
						this.props.data.fragments_2.map((fragment, index) => (
							<div draggable="true" onDragStart={this.handleDragStart} onDragEnd={this.handleDragEnd} className="d-flex align-items-center justify-content-center text-center" onClick={this.fragmentToggleShading} key={this.props.data.name + "d2" + index} id={this.props.data.name + "d2" + index}>
								<span key={this.props.data.name + "p2" + index}>{fragment.toUpperCase()}</span>
							</div>
						))
					}
					</div>
					<h3 className="pt-2">Length 3:</h3>
					<div className="fragment-display d-flex align-items-center justify-content-center text-center">
					{
						this.props.data.fragments_3.map((fragment, index) => (
							<div draggable="true" onDragStart={this.handleDragStart} onDragEnd={this.handleDragEnd} className="d-flex align-items-center justify-content-center text-center" onClick={this.fragmentToggleShading} key={this.props.data.name + "d3" + index} id={this.props.data.name + "d3" + index}>
								<span key={this.props.data.name + "p3" + index}>{fragment.toUpperCase()}</span>
							</div>
						))
					}
					</div>
				</div>

				<div className="col-12 col-sm-12 col-md-12 col-lg-6">
					<h3 className="pb-4 border-bottom">Solution</h3>
					{
						Array.from(this.props.data.solution_boxes).map((answer,index) => {
							return <PuzzlePiece key={this.props.data.name + "sol" + index} puzzleId={this.props.puzzleId} readOnly="true" hook={this.props.puzzleId + answer} check_puzzle={this.check_puzzle} />
						})
					}
				</div>
			</div>
		</div>;
	}

}

class Puzzle_WordSearch extends Puzzle
{
	constructor(props) {
		super(props);
		
		for (var ind = 0; ind < this.props.data.clues.length; ind ++)
		{
			this.state.correct_answers += this.props.data.clues[ind].answer.toUpperCase();
		}

	}

	render() {
		return <div className={"container puzzle " + this.props.visible + " " + this.props.puzzleId}>
			{this.common_render()}
			<div className="row">
				<div className="py-4 col-12 col-sm-12 col-md-12 col-lg-6">
					<h3 className="pb-4 border-bottom">{this.props.data.directions}</h3>
					
					<div className="pt-2">
					{
						this.props.data.clues.map((clue, index) => (
							<div className="wordsearch-div" key={this.props.data.name + "div" + index}>
								<PuzzlePiece key={this.props.data.name + "cl" + index} puzzleId={this.props.puzzleId} readOnly="false" checkAnswers="true" hook={this.props.puzzleId + String.fromCharCode(65 + index) + this.state.number_of_letters++} check_puzzle={this.check_puzzle}/>
								<span key={this.props.data.name + "p" + index}>{clue.clue.toUpperCase()}</span>
							</div>
						))
					}
					</div>
				</div>
				<div className="py-4 col-12 col-sm-12 col-md-12 col-lg-6">
					<h3 className="pb-4 border-bottom">Grid of Letters</h3>
					<div className="py-2 border-bottom">
						<table className="wordsearch-table">
							<tbody>
								{
									this.props.data.grid.map((line, outer_index) => (
										<tr key={this.props.data.name + "tr" + outer_index}>
											{
												Array.from(line).map((letter, inner_index) =>(
													<td key={this.props.data.name + "td" + outer_index + inner_index}>{letter.toUpperCase()}</td>
												))
											}
										</tr>
									))
								}
							</tbody>
						</table>
					</div>

					<h3 className="py-4 border-bottom">Solution</h3>
					{
						this.props.data.clues.map((clue, index) => {
							return <PuzzlePiece key={this.props.data.name + "ans" + index} puzzleId={this.props.puzzleId} hook={this.props.puzzleId + String.fromCharCode(65 + index) + (index + 1).toString()} readOnly="true" check_puzzle={this.check_puzzle} />
						})
					}

				</div>
			</div>
				
			</div>;
	}

}

class Puzzle_Box extends Puzzle
{
	constructor(props) {
		super(props);
		
		for (var ind = 0; ind < this.props.data.clues.length; ind ++)
		{
			this.state.correct_answers += this.props.data.clues[ind].answer.toUpperCase();
		}

	}

	render() {
		return <div className={"container puzzle " + this.props.visible + " " + this.props.puzzleId}>
			{this.common_render()}
				<div className="row py-4">
					<h3 className="pb-4 border-bottom">{this.props.data.directions}</h3>
					<div className="pt-2">
					<table className="box-table">
						<thead>
							<tr className="h-100">
								<th className="h-100" scope="col"></th>
								{
									Array.from(this.props.data.clues[0].answer).map((letter, index) => (
										<th key={this.props.data.name+"th"+index}>
											<div className="h-100 d-flex align-items-center justify-content-center">
											{index == 0 && <PuzzlePiece key={this.props.data.name + "ans" + index} puzzleId={this.props.puzzleId} hook={this.props.puzzleId + String.fromCharCode(60 + index) + (1 + index * this.props.data.clues[0].answer.length).toString()} readOnly="true" value={this.props.data.first_letter.toUpperCase()} check_puzzle={this.check_puzzle} />}
											{index > 0 && <PuzzlePiece key={this.props.data.name + "ans" + index} puzzleId={this.props.puzzleId} hook={this.props.puzzleId + String.fromCharCode(65 + index - 1) + (1 + (index - 1) * this.props.data.clues[0].answer.length).toString()} readOnly="true" check_puzzle={this.check_puzzle} />}
											</div>
										</th>
									))
								}
							</tr>
						</thead>
						<tbody>
							{
								this.props.data.clues.map((clue, outer_index) => (
									<tr className="h-100" key={this.props.data.name+"tr"+outer_index}>
										<td className="h-100 box-traversal-clue-td" key={this.props.data.name+"td"+outer_index}>{clue.clue}</td>
										{
											Array.from(clue.answer).map((letter, inner_index) => (
												<td className="h-100 box-td" key={this.props.data.name+"td"+outer_index+inner_index}>
													<div className="h-100 d-flex align-items-center justify-content-center" key={this.props.data.name+"div"+outer_index+inner_index}>
														<PuzzlePiece key={this.props.data.name + "p" + outer_index + inner_index} puzzleId={this.props.puzzleId} hook={this.props.puzzleId + String.fromCharCode(65 + outer_index) + this.state.number_of_letters++} readOnly="false" checkAnswers="true" check_puzzle={this.check_puzzle} />
													</div>
												</td>
											))
										}
									</tr>
								))
							}
						</tbody>
					</table>
					</div>
				</div>
				
			</div>;
	}

}

class Puzzle_Box_Traversal extends Puzzle
{
	constructor(props) {
		super(props);
		
		for (var ind = 0; ind < this.props.data.clues.length; ind ++)
		{
			this.state.correct_answers += this.props.data.clues[ind].answer.toUpperCase();
		}

		this.state.correct_answers += this.props.data.final_answer.replace(/\s/g, '').toUpperCase();
	}

	render() {
		return <div className={"container puzzle " + this.props.visible + " " + this.props.puzzleId}>
			{this.common_render()}
			<div className="row">
				<div className="py-4 col-12 col-sm-12 col-md-12 col-lg-6">
					<h3 className="pb-2 border-bottom">{this.props.data.directions}</h3>
					<table className="table box-traversal-table">
						<tbody>
							{
								this.props.data.clues.map((clue, outer_index) => (
									<tr className="h-100" key={this.props.data.name+"tr"+outer_index}>
										<td className="h-100 box-traversal-clue-td" key={this.props.data.name+"td"+outer_index}>{clue.clue}</td>
										{
											Array.from(clue.answer).map((letter, inner_index) => (
												<td className="h-100 box-traversal-direction-td" key={this.props.data.name+"td"+outer_index+inner_index}>
													<div className="row h-100 justify-content-center align-items-center" key={this.props.data.name+"div"+outer_index+inner_index}>
														<div className="h-100 d-flex align-items-center col-6">
															<PuzzlePiece key={this.props.data.name + "p" + outer_index + inner_index} puzzleId={this.props.puzzleId} hook={this.props.puzzleId + String.fromCharCode(65 + outer_index) + this.state.number_of_letters++} readOnly="false" checkAnswers="true" check_puzzle={this.check_puzzle} />
														</div>

														<div className="d-flex align-items-center justify-content-center col-6">
														{this.props.data.box_structure[outer_index][inner_index] != "" &&
															<div className="container">
																<div className="row justify-content-center">
																	<div className="col-6 text-center">
																		<span className="box-traversal-direction">{this.props.data.box_structure[outer_index][inner_index][0]}</span>
																	</div>
																
																	<div className="col-6 text-center pr-2">
																	{this.props.data.box_structure[outer_index][inner_index][1] == 'u' && <i className="bi bi-arrow-up"></i>}
																	{this.props.data.box_structure[outer_index][inner_index][1] == 'd' && <i className="bi bi-arrow-down"></i>}
																	{this.props.data.box_structure[outer_index][inner_index][1] == 'l' && <i className="bi bi-arrow-left"></i>}
																	{this.props.data.box_structure[outer_index][inner_index][1] == 'r' && <i className="bi bi-arrow-right"></i>}
																	</div>
																</div>
															</div>
														}
														{
															this.props.data.box_structure[outer_index][inner_index] == "" &&
															<div className="container">
																<div className="row">
																	<div className="col-6 text-center">
																	<span className="box-traversal-direction">{this.props.data.box_structure[outer_index][inner_index][0]}</span>
																	</div>
																	<div className="col-6 text-center pr-2">
																	<span className="box-traversal-direction"> </span>
																	</div>
																</div>
															</div>
														}
													</div>
													</div>
												</td>
											))
										}
									</tr>
								))
							}
						</tbody>
					</table>
				</div>
				<div className="py-4 col-12 col-sm-12 col-md-12 col-lg-1"> </div>
				<div className="py-4 col-12 col-sm-12 col-md-12 col-lg-5">
					<h3 className="pb-2 border-bottom">Solution</h3>
					<div className="pt-2">
					{
						Array.from(this.props.data.final_answer).map((answer,index) => {
							if (answer == " ")
							{
								return <div key={this.props.data.name + "hd" + index} className="horizontalgap"> </div>;
							}	
							else
							{
								return <PuzzlePiece key={this.props.data.name + "ca" + index} puzzleId={this.props.puzzleId} readOnly="false" checkAnswers="true" hook={this.props.puzzleId + String.fromCharCode(65 + index) + this.state.number_of_letters++} check_puzzle={this.check_puzzle} />
							}
						})
					}
					</div>
				</div>
			</div>
		</div>;
	}

}

class Puzzle_Linked_FITB extends Puzzle 
{
	constructor(props) {
		super(props);
		
		for (var ind = 0; ind < this.props.data.section_1_clues.length; ind ++)
		{
			this.state.correct_answers += this.props.data.section_1_clues[ind].answer.toUpperCase();
		}
	}

	render() {
		return <div className={"container puzzle " + this.props.visible + " " + this.props.puzzleId}>
			{this.common_render()}
			<div className="row">
				<div className="py-4 col-12 col-sm-12 col-md-12 col-lg-6">
					<h3 className = "pb-2 border-bottom">{this.props.data.section_1_directions}</h3>
					{
						this.props.data.section_1_clues.map((clue, outer_index) => (
							<div className="border-bottom pb-2" key={this.props.data.name + "d" + outer_index}>
								<h4 key={this.props.data.name + "c" + outer_index}>{clue.clue}</h4>
								{
									Array.from(clue.answer).map((letter, inner_index) => (
										<PuzzlePiece key={this.props.data.name + "c" + outer_index + inner_index} puzzleId={this.props.puzzleId} hook={this.props.puzzleId + String.fromCharCode(65 + outer_index) + this.state.number_of_letters++} readOnly="false" checkAnswers="true" check_puzzle={this.check_puzzle} />
									))
								}
							</div>
						))
					}
				</div>
				<div className="py-4 col-12 col-sm-12 col-md-12 col-lg-6">
					<h3 className = "pb-2 border-bottom">{this.props.data.section_2_directions}</h3>
					<div className="pt-2">
					{
						this.props.data.section_2_answers.map((answer, index) => {
							if (answer == " ")
							{
								return <div key={this.props.data.name + "hd" + index} className="horizontalgap"> </div>;
							}	
							else
							{
								return <PuzzlePiece key={this.props.data.name + "ca" + index} puzzleId={this.props.puzzleId} hook={this.props.puzzleId + answer.toUpperCase()} readOnly="true" check_puzzle={this.check_puzzle} />
							}
						})
					}
					</div>
				</div>
			</div>
		</div>;
	}
}


class App extends React.Component 
{
	constructor(props) {
		super(props);
		this.state = {currentPuzzle: 0};
		this.switchPuzzle = this.switchPuzzle.bind(this);
	}

	switchPuzzle(e) {

		var classToFind = e.target.innerText.replace(/\s/g, '').toLowerCase();

		var elements = document.querySelectorAll('div.puzzle');
		for (var ind = 0; ind < elements.length ; ind ++)
		{
			var element = elements[ind];

			if (element.classList.contains(classToFind))
			{
				// element.classList.remove("gone");
				element.classList.remove("invisible");
				element.classList.add("visible");
			}
			else
			{
				element.classList.add("invisible");
				element.classList.remove("visible");
			}
		}

		elements = document.querySelectorAll('a.nav-link');
		for (var ind = 0; ind < elements.length ; ind ++)
		{
			var element = elements[ind];
			element.classList.remove("active");
		}
		e.target.classList.add("active");

// 		setTimeout(function(){
// 			
// 			var elements = document.querySelectorAll('div.puzzle');
// 			for (var ind = 0; ind < elements.length; ind ++)
// 			{
// 				var element = elements[ind];
// 
// 				if (element.classList.contains("invisible"))
// 				{
// 					element.classList.add("gone");
// 				}
// 
// 			}
// 		}.bind(this), 500);

	}

	render() {
		return (
			<div className="container">
				<header className="d-flex flex-wrap justify-content-center mb-2 mt-2 pb-2 border-bottom">
					<ul className="nav nav-pills">
						{
							puzzle_data.puzzles.map((puzzle, index) => (
								<li key={puzzle.name + "_li"} className="nav-item">
									<a className={"nav-link " + ((index == this.state.currentPuzzle) ? "active" : "")} key={puzzle.name + "_button"} onClick={this.switchPuzzle} type="button">{puzzle.name}</a>
								</li>
							))
						}
					</ul>
				</header>
				
				{
					puzzle_data.puzzles.map((puzzle, index) => {
						switch(puzzle.type)
						{
							case "linked_fitb":
								return <Puzzle_Linked_FITB key={puzzle.name} data={puzzle} puzzleId={puzzle.name.replace(/\s/g, '').toLowerCase()} visible={this.state.currentPuzzle == index ? "visible" : "invisible"} />;
							case "box_traversal":
								return <Puzzle_Box_Traversal key={puzzle.name} data={puzzle} puzzleId={puzzle.name.replace(/\s/g, '').toLowerCase()} visible={this.state.currentPuzzle == index ? "visible" : "invisible"} />;
							case "box":
								return <Puzzle_Box key={puzzle.name} data={puzzle} puzzleId={puzzle.name.replace(/\s/g, '').toLowerCase()} visible={this.state.currentPuzzle == index ? "visible" : "invisible"} />;
							case "wordsearch":
								return <Puzzle_WordSearch key={puzzle.name} data={puzzle} puzzleId={puzzle.name.replace(/\s/g, '').toLowerCase()} visible={this.state.currentPuzzle == index ? "visible" : "invisible"} />;
							case "fragment":
								return <Puzzle_Fragment key={puzzle.name} data={puzzle} puzzleId={puzzle.name.replace(/\s/g, '').toLowerCase()} visible={this.state.currentPuzzle == index ? "visible" : "invisible"} />;
							case "box_simple":
								return <Puzzle_Box_Simple key={puzzle.name} data={puzzle} puzzleId={puzzle.name.replace(/\s/g, '').toLowerCase()} visible={this.state.currentPuzzle == index ? "visible" : "invisible"} />;
							case "box_sum":
								return <Puzzle_Box_Sum key={puzzle.name} data={puzzle} puzzleId={puzzle.name.replace(/\s/g, '').toLowerCase()} visible={this.state.currentPuzzle == index ? "visible" : "invisible"} />;
						}
					})
				}
			</div>
		);
	}
};

class AppHeader extends React.Component 
{
	render() {
		return (
			<div className="container">
				<header className="d-flex flex-wrap justify-content-center py-3 border-bottom">
					<div className="container">
						<div className="row">
							<div className="col-12 col-sm-12 col-md-12 col-lg-4">
								<a className="d-flex align-items-center mb-3 mb-md-0 me-md-auto text-dark text-decoration-none" href="/">
									<img className="me-2" src="./logo.png" />
									<span className="fs-4">{puzzle_data.puzzle_metadata.header}</span>
								</a>
							</div>
							<div className="col-12 col-sm-12 col-md-12 col-lg-6 d-flex">
								<span className="align-items-center fs-8 d-flex" id="puzzle-quote">{puzzle_data.puzzle_metadata.quote}</span>
							</div>
							<div className="col-12 col-sm-12 col-md-12 col-lg-2 d-flex flex-column">
								<span className="fs-8 d-flex justify-content-end align-items-center">{puzzle_data.puzzle_metadata.subheader}</span>
								<span className="fs-8 d-flex justify-content-end align-items-center">v.0.0.1</span>
							</div>
						</div>
					</div>					
				</header>
			</div>
		);
	}
};


ReactDOM.render(
	React.createElement(AppHeader),
	document.querySelector('#AppHeader')
);


ReactDOM.render(
	React.createElement(App),
	document.querySelector('#App')
);




// Helper function for drag and drop.
function updateCharacter(el, t) {

	if (el == null) return;

	el.lastChild.value = t;
	
	var elements = document.querySelectorAll('div.puzzle.visible input.puzzle-piece.puzzle-piece-' + el.dataset["hook"]);
	for (var ind = 0 ; ind < elements.length ; ind ++)
	{
		var element = elements[ind];
		if (element == el.lastChild) continue;
		if (element.value != t)
		{
			element.value = t;
		}
	}
}



const UPPERCASE_PREFIX = '^{';
const UPPERCASE_SUFFIX = '}^';

function encodeUpperCase(str)  {
	return str.replace(/([A-Z]+)/g, `${UPPERCASE_PREFIX}$1${UPPERCASE_SUFFIX}`);
}

function decodeUpperCase(str)  {
	const escapeRegExp = (escape) => ['', ...escape.split('')].join('\\');

	return str.replace(
	  new RegExp(`${escapeRegExp(UPPERCASE_PREFIX)}(.*?)${escapeRegExp(UPPERCASE_SUFFIX)}`, 'g'),
	  (_, p1) => p1.toUpperCase()
	);
}