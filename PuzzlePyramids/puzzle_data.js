const puzzle_data = {
	"puzzle_metadata":
	{
		"header":"Puzzle Pyramid: 4/2/2021",
		"subheader":"By Chip Beauvais © 2021",
		"quote": "(solve puzzle J) (solve puzzle K) is the best (solve puzzle L). -George Herbert"
	},
	"puzzles":	
	[
		{
			"name": "Puzzle A",
			"type": "linked_fitb",
			"section_1_directions": "Fill in the blanks below based on the clues.",
			"section_1_clues":
			[
				{"clue": "Worm on a hook, say", "answer": "bait"},
				{"clue": "Title for a married woman", "answer": "mrs"},
				{"clue": "Bean-shaped organ", "answer": "kidney"},
				{"clue": "Confirm", "answer": "verify"},
				{"clue": "Compelled", "answer": "forced"},
				{"clue": "Doe or buck", "answer": "deer"}
			],
			"section_2_directions": "Then copy the letters to the grid below.",
			"section_2_answers":
			[
				"b5", "a2", "b6", "c8", "c12", "c10", " ", "a1", "c13", " ", "e25", "a3", "d14", "d15", "d16", "b7", "c9", "a4", "d19", " ", "e21", "e22", " ", "f26", "d17", "d18", "e20", "e24", "f29", "f27", "c11", "e23", "f28"
			],
			"final_answer": "marked by diversity or difference"
		},
		{
			"name": "Puzzle B",
			"type": "box_traversal",
			"directions": "Fill in the words below (one letter per box) based on the clues. Starting in the top left box, follow the direction (e.g. three spaces to the right to find the next letter.",
			"clues":
			[
				{"clue": "Highlander's family", "answer": "clan"},
				{"clue": "Present", "answer": "gift"},
				{"clue": "Broadcasts", "answer": "airs"},
				{"clue": "Longstanding fight, possibly between families", "answer": "feud"}
			],
			"box_structure":
			[
				["2d",   "", "1d", "2d"],
				["2r", "1r", "2d", "1u"],
				["2r", "1u", "1u", "1l"],
				["1r", "3u", "1l", "1u"]
			],
			"final_answer": "car fuel"
		},
		{
			"name": "Puzzle C",
			"type": "box",
			"directions": "Use the clues below to fill in the grid. Each horizontal word also appears vertically (in the same order). Then read the solution to the puzzle from the highlighted squares.",
			"clues":
			[
				{"clue": "Lukewarm", "answer": "tepid"},
				{"clue": "Copying", "answer": "aping"},
				{"clue": "Bit of color", "answer": "tinge"},
				{"clue": "Trimmed, as a lawn", "answer": "edged"}
			],
			"first_letter": "S",
			"final_answer": "state"
		},
		{
			"name": "Puzzle D",
			"type": "wordsearch",
			"directions": "This is a word search, more or less.\n\nEach word listed below is almost (give or take one letter hidden in the grid.\n\nFor example, if HEAR is given as a word, you might find HER in the grid, or you might find HEART.\n\nWrite the letter that was added (or removed) next to the word, and then read down the column of letters to solve the puzzle.",
			"clues":
			[
				{"clue": "rim", "answer": "t"},
				{"clue": "bar", "answer": "o"},
				{"clue": "wad", "answer": "n"},
				{"clue": "ruby", "answer": "y"}
			],
			"grid":
			[
				"buru",
				"mirt",
				"dnaw",
				"boar"
			],
			"final_answer": "tony"
		},
		{
			"name": "Puzzle E",
			"type": "fragment",
			"directions": "Construct the missing words by using the 2 or 3 letter fragments below. Each fragment will be used once.\nMove the letters (in order) from the shaded boxes to the solution below.",
			"clues":
			[
				{"clue": "A machine for performing calculations automatically", "answer": "computer"},
				{"clue": "May be early", "answer": "adopter"},
				{"clue": "Taking the day off, possibly", "answer": "relaxing"}
			],
			"fragments_2": ["ax", "er", "pt", "pu"],
			"fragments_3": ["ado", "com", "ing", "rel", "ter"],
			"final_answer": "murder",
			"solution_boxes": ["A3", "A5", "A8", "B10", "B14", "C16"]
		},
		{
			"name": "Puzzle F",
			"type": "box_simple",
			"directions": "Fill in the clues below, and then read the solution down the shaded column.",
			"clues":
			[
				{"clue": "Any epidemic disease with a high death rate", "answer": "plague"},
				{"clue": "Opposite of danger", "answer": "safety"},
				{"clue": "Marked by injustice", "answer": "unfair"},
				{"clue": "Drizzled or poured", "answer": "abcdef"},
				{"clue": "Small rodent", "answer": "gerbil"},
				{"clue": "Temporary tent dweller", "answer": "camper"},
			],
			"solution_column": 3,
			"final_answer": "abcdef"
		},
		{
			"name": "Puzzle G",
			"type": "linked_fitb",
			"section_1_directions": "Fill in the blanks below based on the clues.",
			"section_1_clues":
			[
				{"clue": "Grain tower", "answer": "silo"},
				{"clue": "Make a mistake", "answer": "err"},
				{"clue": "Charged particle", "answer": "ion"},
				{"clue": "Excuse", "answer": "pardon"},
				{"clue": "Opposite of feast", "answer": "famine"},
				{"clue": "Skeleton's site?", "answer": "closet"}
			],
			"section_2_directions": "Then copy the letters to the grid below.",
			"section_2_answers":
			[
				"f23","a4","c10","d14","a2","f28","c8","c9","d16","d12","a3"," ","b6","b5","f24","e22","e18","a1","f27"," ","e17","b7","d15","e19"," ","d11","d13","e20","f26","f25","e21"
			],
			"final_answer": "conditional release from prison"
		},
		{
			"name": "Puzzle H",
			"type": "box_traversal",
			"directions": "Fill in the words below (one letter per box) based on the clues. Starting in the top left box, follow the direction (e.g. three spaces to the right to find the next letter.",
			"clues":
			[
				{"clue": "'So long,' in Sevilla", "answer": "adios"},
				{"clue": "Evil spirit", "answer": "demon"},
				{"clue": "Remove, as a beard", "answer": "shave"},
				{"clue": "A machine used for printing", "answer": "press"},
				{"clue": "Covers, as a driveway", "answer": "paves"}
			],
			"box_structure":
			[
				["4d", "3d", "4d", "1l", "2l"],
				["1r", "1d", "1r", "1r", "1u"],
				["2r", "3r", "1u", "1l", "1u"],
				["1r", "2u",   "", "1d", "3l"],
				["1u", "1l", "1u", "2u", "1l"]
			],
			"final_answer": "apprehensive"
		},
		{
			"name": "Puzzle I",
			"type": "fragment",
			"directions": "Construct the missing words by using the 2 or 3 letter fragments below. Each fragment will be used once.\nMove the letters (in order) from the shaded boxes to the solution below.",
			"clues":
			[
				{"clue": "School staff", "answer": "faculty"},
				{"clue": "Better at retaining liquid", "answer": "spongier"},
				{"clue": "Resin used in making celluloid", "answer": "camphor"},
				{"clue": "Preparing", "answer": "readying"}
			],
			"fragments_2": ["ca", "fa", "mp", "ng", "re", "ty"],
			"fragments_3": ["ady", "cul", "hor", "ier", "ing", "spo"],
			"final_answer": "autographed",
			"solution_boxes": ["A2", "A4", "A6", "B10", "B12", "B15", "C17", "C19", "C20", "D24", "D26"]
		},
		{
			"name": "Puzzle J",
			"type": "fragment",
			"directions": "Construct the missing words by using the 2 or 3 letter fragments below. Each fragment will be used once.\nMove the letters (in order) from the shaded boxes to the solution below.",
			"clues":
			[
				{"clue": "(Solve Puzzle C)", "answer": "aaaaaaaa"},
				{"clue": "(Solve Puzzle A)", "answer": "aaaaaaa"},
				{"clue": "(Solve Puzzle B)", "answer": "aaaaaaaa"}
			],
			"fragments_2": ["ga", "ng", "pr", "va"],
			"fragments_3": ["aim", "ine", "ocl", "ryi", "sol"],
			"final_answer": "aaaaaa",
			"solution_boxes": ["A5", "A7", "B9", "B13", "B14", "C16"]
		},
		{
			"name": "Puzzle K",
			"type": "box_sum",
			"directions": "Each word below matches at least two numbered clues. The number next to each word is the total of all of the clues to that word.\n\nFor example, it clues 1 and 4 describe the same word, that word will appear next to the number 5.\n\nAfter you have filled in all the words, read down the column of letters to get the solution.",
			"clues":
			[
				{"clue": "Opposite of foreit", "value": 1},
				{"clue": "(Solve Puzzle D)", "value": 2},
				{"clue": "Give as judged due or on the basis of merit", "value": 3},
				{"clue": "Oscar", "value": 4},
				{"clue": "Marsh plants", "value": 5},
				{"clue": "(Solve Puzzle E)", "value": 6},
				{"clue": "Off", "value": 7},
				{"clue": "Oboe bits", "value": 8},
				{"clue": "Leave laughing in the aisles", "value": 9},
				{"clue": "(Solve Puzzle F)", "value": 10},
			],
			"solution_lengths":[9,13,22,11],
			"solution_column": 2,
			"answers":["aaaaa","aaaaa","aaaa","aaaaa"],
			"final_answer": "aaaa",
			"solution_boxes": ["A2", "B7", "C12", "D16"]
		},
		{
			"name": "Puzzle L",
			"type": "box_simple",
			"directions": "Fill in the clues below, and then read the solution down the shaded column.",
			"clues":
			[
				{"clue": "(Solve Puzzle G)", "answer": "abcdef"},
				{"clue": "With stealth", "answer": "abcdef"},
				{"clue": "Eat", "answer": "abcdef"},
				{"clue": "(Solve Puzzle H)", "answer": "abcdef"},
				{"clue": "Small boat", "answer": "abcdef"},
				{"clue": "(Solve Puzzle I)", "answer": "abcdef"},
				{"clue": "Satisfy (thirst)", "answer": "abcdef"}
			],
			"solution_column": 3,
			"final_answer": "abcdefg"
		}
	]
}