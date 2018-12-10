using System;
using System.ComponentModel;

namespace NumberGenerator.Logic
{
    /// <summary>
    /// Beobachter, welcher die Zahlen auf der Konsole ausgibt.
    /// Diese Klasse dient als Basisklasse für komplexere Beobachter.
    /// </summary>
    public class BaseObserver
    {
        #region Fields
        private readonly RandomNumberGenerator _numberGenerator;
        #endregion

        #region Properties

        public int CountOfNumbersReceived { get; private set; }
        public int CountOfNumbersToWaitFor { get; private set; }

        #endregion

        #region Constructors

        public BaseObserver(RandomNumberGenerator numberGenerator, int countOfNumbersToWaitFor)
        {
            if (countOfNumbersToWaitFor < 0)
            {
                throw new ArgumentException("Negative Wartezeiten sind nicht zulässig!");
            }
            else
            {
                _numberGenerator = numberGenerator;
                CountOfNumbersToWaitFor = countOfNumbersToWaitFor;

                //_numberGenerator.Attach(this);
                _numberGenerator.NumberChanged += OnNextNumber;
            }
        }

        #endregion

        #region Methods

        #region IObserver Members

        /// <summary>
        /// Wird aufgerufen wenn der NumberGenerator eine neue Zahl generiert hat.
        /// </summary>
        /// <param name="number"></param>
        public virtual void OnNextNumber(object sender, int number)
        {
            CountOfNumbersReceived++;

            // Sobald die Anzahl der max. Beobachtungen (_countOfNumbersToWaitFor) erreicht ist -> Detach()
            if (CountOfNumbersReceived >= CountOfNumbersToWaitFor)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"   >> {this.GetType().Name}: Received '{CountOfNumbersReceived}' of '{CountOfNumbersToWaitFor}' => I am not interested in new numbers anymore => Detach().");
                Console.ResetColor();
                DetachFromNumberGenerator();
            }
        }

        #endregion

        public override string ToString()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Von Liste abmelden
        /// </summary>
        protected void DetachFromNumberGenerator()
        {
            _numberGenerator.NumberChanged -= OnNextNumber;
        }

        #endregion
    }
}
